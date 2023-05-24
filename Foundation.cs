﻿using FAC.Compontents.Auxiliaries;
using FAC.Compontents.Finders;
using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using System.IO;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Terraria.ObjectData;

namespace FAC
{
    public abstract class Foundation : ModTileEntity
    {
        readonly List<Compontent> _compontents = new();
        public bool AddCompontent(Compontent compontent)
        {
            if (!_compontents.All(c => c.CompatibleWith(compontent)))
            {
                return false;
            }
            _compontents.Add(compontent);
            compontent.Foundation = this;
            compontent.OnEquip();
            return true;
        }
        public bool RemoveCompontent(Compontent compontent)
        {
            return _compontents.Remove(compontent);
        }
        public bool ContainsCompontent(Compontent compontent)
        {
            return _compontents.Contains(compontent);
        }
        public List<T> GetCompontents<T>() where T : Compontent
        {
            List<T> result = new();
            foreach (Compontent c in _compontents)
            {
                if (c is T t)
                {
                    result.Add(t);
                }
            }
            return result;
        }
        public bool TryGetTarget<T>(out T target)
        {
            foreach (var c in GetCompontents<Finder<T>>())
            {
                if (c.TryFindTarget(out target))
                {
                    return true;
                }
            }
            target = default;
            return false;
        }
        public void ApplyAuxiliaryTo(Compontent compontent)
        {
            foreach (Auxiliary a in GetCompontents<Auxiliary>())
            {
                if (a.CanApplyTo(compontent))
                {
                    a.Apply(compontent);
                }
            }
        }
        public override bool IsTileValidForEntity(int x, int y)
        {
            return Main.tile[x, y].HasTile && TileLoader.GetTile(Main.tile[x, y].TileType) is FoundationTile<Foundation>;
        }
        public override void Update()
        {
            foreach (Compontent compontent in _compontents)
            {
                compontent.Update();
            }
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(_compontents.Count);
            foreach (Compontent compontent in _compontents)
            {
                writer.Write(compontent.Item, true, true);
            }
        }
        public override void NetReceive(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            _compontents.Clear();
            for (int i = 0; i < count; i++)
            {
                _compontents.Add((Compontent)reader.ReadItem(true, true).ModItem);
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag[nameof(_compontents)] = _compontents.ConvertAll(c => c.Item);
        }
        public override void LoadData(TagCompound tag)
        {
            _compontents.Clear();
            if (tag.TryGet(nameof(_compontents), out List<Item> items))
            {
                items.ForEach(item => _compontents.Add((Compontent)item.ModItem));
            }
        }
        public override int Hook_AfterPlacement(int i, int j, int placeType, int style, int direction, int alternate)
        {
            TileObjectData data = TileObjectData.GetTileData(Main.tile[i,j].TileType, style, alternate);
            if(data is null)
            {
                throw new Exception("This foundation needs to be bound to ModTiles which have TileObjectData registered");
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i - data.Origin.X, j - data.Origin.Y, data.Width, data.Height);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i - data.Origin.X, j - data.Origin.Y, Type);
                return -1;
            }
            int placedEntity = Place(i - 3, j - 3);
            if (ByID[placedEntity] is Foundation foundation)
            {
                foundation._compontents.Clear();
            }
            return placedEntity;
        }
        public override void OnNetPlace()
        {
            _compontents.Clear();
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
        }
        public static Foundation Get(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return !tile.HasTile ? null : !Extensions.TryGetTileEntityAs<Foundation>(x, y, out var te) ? null : te;
        }
        public static bool TryGet(out Foundation foundation, int x,int y)
        {
            foundation = Get(x,y);
            return foundation is not null;
        }
    }
}