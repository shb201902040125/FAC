using FAC.Compontents.Auxiliaries;
using FAC.Compontents.Finders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace FAC
{
    public abstract class Foundation : ModTileEntity
    {
        public override bool IsLoadingEnabled(Mod mod) => mod is not FAC;
        private readonly List<Compontent> _compontents = new();
        public IReadOnlyList<Compontent> Compontents => _compontents;
        public Guid UID { get; private set; } = new();
        public Vector2 Center
        {
            get
            {
                TileObjectData data = TileObjectData.GetTileData(Main.tile[Position.X, Position.Y]);
                if (data is null)
                {
                    throw new Exception("This foundation needs to be bound to ModTiles which have TileObjectData registered");
                }
                var pos = Extensions.GetTileOrigin(Position.X, Position.Y);
                return new Vector2(pos.X, pos.Y) * 16 + new Vector2(data.Width, data.Height) * 8;
            }
        }
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
            if (compontent.Foundation == this)
            {
                compontent.Foundation = null;
            }
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
            tag[nameof(UID)] = UID.ToString();
            tag[nameof(_compontents)] = _compontents.ConvertAll(c => c.Item);
        }
        public override void LoadData(TagCompound tag)
        {
            _compontents.Clear();
            UID = tag.TryGet(nameof(UID), out string uid) ? Guid.Parse(uid) : (new());
            if (tag.TryGet(nameof(_compontents), out List<Item> items))
            {
                items.ForEach(item => _compontents.Add((Compontent)item.ModItem));
            }
        }
        public override int Hook_AfterPlacement(int i, int j, int placeType, int style, int direction, int alternate)
        {
            TileObjectData data = TileObjectData.GetTileData(Main.tile[i, j].TileType, style, alternate);
            if (data is null)
            {
                throw new Exception("This foundation needs to be bound to ModTiles which have TileObjectData registered");
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i - data.Origin.X, j - data.Origin.Y, data.Width, data.Height);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i - data.Origin.X, j - data.Origin.Y, Type);
                return -1;
            }
            int placedEntity = Place(i - data.Origin.X, j - data.Origin.Y);
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
            return !tile.HasTile ? null : !Extensions.TryGetTileEntityAs(x, y, out Foundation te) ? null : te;
        }
        public static bool TryGet(out Foundation foundation, int x, int y)
        {
            foundation = Get(x, y);
            return foundation is not null;
        }
        public virtual bool FACPreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return !_compontents.Any() || _compontents.All(c => c.FACPreDraw(i, j, spriteBatch));
        }
        public virtual void FACPostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            foreach (var c in _compontents)
            {
                c.FACPostDraw(i, j, spriteBatch);
            }
        }
        public virtual void FACDrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            foreach (var c in _compontents)
            {
                c.FACDrawEffects(i, j, spriteBatch, ref drawData);
            }
        }
        public virtual void FACSetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            foreach (var c in _compontents)
            {
                c.FACSetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tileFrameX, ref tileFrameY);
            }
        }
        public virtual void FACSpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            foreach (var c in _compontents)
            {
                c.FACSpecialDraw(i, j, spriteBatch);
            }
        }
        /// <summary>
        /// 全部组件都返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACCanExplode(int i, int j)
        {
            if (_compontents.Count == 0)
            {
                return true;
            }
            foreach (var c in _compontents)
            {
                if (!c.FACCanExplode(i, j))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 全部组件都返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="blockDamaged"></param>
        /// <returns></returns>
        public virtual bool FACCanKillTile(int i, int j, ref bool blockDamaged)
        {
            if (_compontents.Count == 0)
            {
                return true;
            }
            foreach (var c in _compontents)
            {
                if (!c.FACCanKillTile(i, j, ref blockDamaged))
                {
                    return false;
                }
            }
            return true;
        }
        public virtual void FACAnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            foreach (var c in _compontents)
            {
                c.FACAnimateIndividualTile(type, i, j, ref frameXOffset, ref frameYOffset);
            }
        }
        /// <summary>
        /// 全部组件都返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool FACAutoSelect(int i, int j, Item item)
        {
            if (_compontents.Count == 0)
            {
                return true;
            }
            foreach (var c in _compontents)
            {
                if (!c.FACAutoSelect(i, j, item))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 全部组件都返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACCanPlace(int i, int j)
        {
            if (_compontents.Count == 0)
            {
                return true;
            }
            foreach (var c in _compontents)
            {
                if (!c.FACCanPlace(i, j))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 全部组件都返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool FACCreateDust(int i, int j, ref int type)
        {
            if (_compontents.Count == 0)
            {
                return true;
            }
            foreach (var c in _compontents)
            {
                if (!c.FACCreateDust(i, j, ref type))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 全部组件都返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACDrop(int i, int j)
        {
            if (_compontents.Count == 0)
            {
                return true;
            }
            foreach (var c in _compontents)
            {
                if (!c.FACDrop(i, j))
                {
                    return false;
                }
            }
            return true;
        }
        public virtual void FACDropCritterChance(int i, int j, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance)
        {
            foreach (var c in _compontents)
            {
                c.FACDropCritterChance(i, j, ref wormChance, ref grassHopperChance, ref jungleGrubChance);
            }
        }
        public virtual ushort FACGetMapOption(int i, int j)
        {
            if (_compontents.Count == 0)
            {
                return 0;
            }
            ushort option = 0;
            foreach (var c in _compontents)
            {
                c.FACGetMapOption(i, j, ref option);
            }
            return option;
        }
        /// <summary>
        /// 任意组件返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public virtual bool FACHasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            if (_compontents.Count == 0)
            {
                return false;
            }
            foreach (var c in _compontents)
            {
                if (c.FACHasSmartInteract(i, j, settings))
                {
                    return true;
                }
            }
            return false;
        }
        public virtual void FACHitWire(int i, int j)
        {
            foreach (var c in _compontents)
            {
                c.FACHitWire(i, j);
            }
        }
        /// <summary>
        /// 任意组件返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACIsLockedChest(int i, int j)
        {
            if (_compontents.Count == 0)
            {
                return false;
            }
            foreach (var c in _compontents)
            {
                if (c.FACIsLockedChest(i, j))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 任意组件返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual bool FACIsTileDangerous(int i, int j, Player player)
        {
            if (_compontents.Count == 0)
            {
                return false;
            }
            foreach (var c in _compontents)
            {
                if (c.FACIsTileDangerous(i, j, player))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 任意组件返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACIsTileSpelunkable(int i, int j)
        {
            if (_compontents.Count == 0)
            {
                return false;
            }
            foreach (var c in _compontents)
            {
                if (c.FACIsTileSpelunkable(i, j))
                {
                    return true;
                }
            }
            return false;
        }
        public virtual void FACKillMultiTile(int i, int j, int frameX, int frameY)
        {
            foreach (var c in _compontents)
            {
                c.FACKillMultiTile(i, j, frameX, frameY);
            }
        }
        /// <summary>
        /// 任意组件返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="fail"></param>
        /// <returns></returns>
        public virtual bool FACKillSound(int i, int j, bool fail)
        {
            if (_compontents.Count == 0)
            {
                return false;
            }
            foreach (var c in _compontents)
            {
                if (c.FACKillSound(i, j, fail))
                {
                    return true;
                }
            }
            return false;
        }
        public virtual void FACKillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            foreach (var c in _compontents)
            {
                c.FACKillTile(i, j, ref fail, ref effectOnly, ref noItem);
            }
        }
        public virtual void FACModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            foreach (var c in _compontents)
            {
                c.FACModifyLight(i, j, ref r, ref g, ref b);
            }
        }
        public virtual void FACModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            foreach (var c in _compontents)
            {
                c.FACModifySittingTargetInfo(i, j, ref info);
            }
        }
        public virtual void FACModifySleepingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            foreach (var c in _compontents)
            {
                c.FACModifySleepingTargetInfo(i, j, ref info);
            }
        }
        public virtual void FACMouseOverFar(int i, int j)
        {
            foreach (var c in _compontents)
            {
                c.FACMouseOverFar(i, j);
            }
        }
        public virtual void FACNearbyEffects(int i, int j, bool closer)
        {
            foreach (var c in _compontents)
            {
                c.FACNearbyEffects(i, j, closer);
            }
        }
        public virtual void FACNumDust(int i, int j, bool fail, ref int num)
        {
            foreach (var c in _compontents)
            {
                c.FACNumDust(i, j, fail, ref num);
            }
        }
        public virtual void FACPlaceInWorld(int i, int j, Item item)
        {
            foreach (var c in _compontents)
            {
                c.FACPlaceInWorld(i, j, item);
            }
        }
        public virtual void FACRandomUpdate(int i, int j)
        {
            foreach (var c in _compontents)
            {
                c.FACRandomUpdate(i, j);
            }
        }
        /// <summary>
        /// 任意组件返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACRightClick(int i, int j)
        {
            if (_compontents.Count == 0)
            {
                return false;
            }
            foreach (var c in _compontents)
            {
                if (c.FACRightClick(i, j))
                {
                    return true;
                }
            }
            return false;
        }
        public virtual void FACSetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            foreach (var c in _compontents)
            {
                c.FACSetSpriteEffects(i, j, ref spriteEffects);
            }
        }
        /// <summary>
        /// 任意组件返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACSlope(int i, int j)
        {
            if (_compontents.Count == 0)
            {
                return false;
            }
            foreach (var c in _compontents)
            {
                if (c.FACSlope(i, j))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 任意组件返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="resetFrame"></param>
        /// <param name="noBreak"></param>
        /// <returns></returns>
        public virtual bool FACTileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            if (_compontents.Count == 0)
            {
                return false;
            }
            foreach (var c in _compontents)
            {
                if (c.FACTileFrame(i, j, ref resetFrame, ref noBreak))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 任意组件返回true时返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="frameXAdjustment"></param>
        /// <param name="dustType"></param>
        /// <param name="manual"></param>
        /// <returns></returns>
        public virtual bool FACUnlockChest(int i, int j, ref short frameXAdjustment, ref int dustType, ref bool manual)
        {
            if (_compontents.Count == 0)
            {
                return false;
            }
            foreach (var c in _compontents)
            {
                if (c.FACUnlockChest(i, j, ref frameXAdjustment, ref dustType, ref manual))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
