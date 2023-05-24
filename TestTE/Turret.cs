﻿using FAC;
using FAC.Compontents.Workers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace FAC.TestTE
{
    internal class Turret : Foundation
    {

    }
    internal class TurretTile : FoundationTile<Turret>
    {
        public override int ItemType => ModContent.ItemType<TurretTileItem>();
        public override void SetStaticDefaults()
        {
            SetFACStaticDefaults();

            TileID.Sets.HasOutlines[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
            TileObjectData.newTile.Origin = new Point16(3, 3);

            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;

            TileObjectData.addTile(Type);
            DustType = 7;
        }
        public override bool RightClick(int i, int j)
        {
            if(Main.LocalPlayer.HeldItem.ModItem is Compontent c)
            {
                Main.NewText(Foundation.Get(i, j) is null) ;//.AddCompontent((Compontent)c.Item.Clone().ModItem);
                //Main.LocalPlayer.HeldItem.TurnToAir();
                return true;
            }
            return base.RightClick(i, j);
        }
    }
    internal class TurretTileItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.createTile = ModContent.TileType<TurretTile>();
        }
    }
    internal class WoodArrow : NPCAttacker
    {
        public override bool AttackNPC(NPC target)
        {
            if(Foundation is null)
            {
                return false;
            }
            Vector2 unit = Vector2.Normalize(target.Center - Foundation.Center);
            Projectile.NewProjectile(new EntitySource_TileEntity(Foundation),
                Foundation.Center,
                unit * 10f,
                ProjectileID.WoodenArrowFriendly,
                Damage,
                Knockback,
                Main.myPlayer);
            return true;
        }
    }
}
