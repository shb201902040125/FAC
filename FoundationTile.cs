using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace FAC
{
    public abstract class FoundationTile<T> : ModTile where T : Foundation
    {
        public override bool IsLoadingEnabled(Mod mod) => mod is not FAC;
        public virtual int ItemType { get; } = -1;
        public override void MouseOver(int i, int j)
        {
            if (ItemType != -1)
            {
                Player player = Main.LocalPlayer;
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = ItemType;
                player.noThrow = 2;
            }
        }
        public void SetFACStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.LavaDeath = false;
            ModTileEntity tileEntity = ModContent.GetInstance<T>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = tileEntity is not null
                ? new PlacementHook(tileEntity.Hook_AfterPlacement, -1, 0, false)
                : new PlacementHook(Hook_AfterPlacement_NoEntity, -1, 0, false);
            //TileID.Sets.DisableSmartCursor[Type] = true;
            //TileID.Sets.HasOutlines[Type] = true;
        }
        public int Hook_AfterPlacement_NoEntity(int i, int j, int type, int style, int direction, int alternate)
        {
            TileObjectData data = TileObjectData.GetTileData(type, style, alternate);
            if (data is null)
            {
                throw new Exception("This foundation needs to be bound to ModTiles which have TileObjectData registered");
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i - data.Origin.X, j - data.Origin.Y, data.Width, data.Height);
            }
            return 0;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACPreDraw(i, j, spriteBatch) : base.PreDraw(i, j, spriteBatch);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACPostDraw(i, j, spriteBatch);
            }
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACDrawEffects(i, j, spriteBatch, ref drawData);
            }
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACSetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tileFrameX, ref tileFrameY);
            }
        }
        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACSpecialDraw(i, j, spriteBatch);
            }
        }
        public override bool CanExplode(int i, int j)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACCanExplode(i, j) : base.CanExplode(i, j);
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACCanKillTile(i, j, ref blockDamaged) : base.CanKillTile(i, j, ref blockDamaged);
        }
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACAnimateIndividualTile(type, i, j, ref frameXOffset, ref frameYOffset);
            }
        }
        public override bool AutoSelect(int i, int j, Item item)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACAutoSelect(i, j, item) : base.AutoSelect(i, j, item);
        }
        public override bool CanPlace(int i, int j)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACCanPlace(i, j) : base.CanPlace(i, j);
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACCreateDust(i, j, ref type) : base.CreateDust(i, j, ref type);
        }
        public override bool Drop(int i, int j)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACDrop(i, j) : base.Drop(i, j);
        }
        public override void DropCritterChance(int i, int j, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACDropCritterChance(i, j, ref wormChance, ref grassHopperChance, ref jungleGrubChance);
            }
        }
        public override ushort GetMapOption(int i, int j)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACGetMapOption(i, j) : base.GetMapOption(i, j);
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACHasSmartInteract(i, j, settings) : base.HasSmartInteract(i, j, settings);
        }
        public override void HitWire(int i, int j)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACHitWire(i, j);
            }
        }
        public override bool IsLockedChest(int i, int j)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACIsLockedChest(i, j) : base.IsLockedChest(i, j);
        }
        public override bool IsTileDangerous(int i, int j, Player player)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACIsTileDangerous(i, j, player) : base.IsTileDangerous(i, j, player);
        }
        public override bool IsTileSpelunkable(int i, int j)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACIsTileSpelunkable(i, j) : base.IsTileSpelunkable(i, j);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACKillMultiTile(i, j, frameX, frameY);
            }
        }
        public override bool KillSound(int i, int j, bool fail)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACKillSound(i, j, fail) : base.KillSound(i, j, fail);
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACKillTile(i, j, ref fail, ref effectOnly, ref noItem);
            }
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACModifyLight(i, j, ref r, ref g, ref b);
            }
        }
        public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACModifySittingTargetInfo(i, j, ref info);
            }
        }
        public override void ModifySleepingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACModifySittingTargetInfo(i, j, ref info);
            }
        }
        public override void MouseOverFar(int i, int j)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACMouseOverFar(i, j);
            }
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACNearbyEffects(i, j, closer);
            }
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACNumDust(i, j, fail, ref num);
            }
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACPlaceInWorld(i, j, item);
            }
        }
        public override void RandomUpdate(int i, int j)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACRandomUpdate(i, j);
            }
        }
        public override bool RightClick(int i, int j)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACRightClick(i, j) : base.RightClick(i, j);
        }
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (Foundation.TryGet(out var f, i, j))
            {
                f.FACSetSpriteEffects(i, j, ref spriteEffects);
            }
        }
        public override bool Slope(int i, int j)
        {
            return Foundation.TryGet(out var f, i, j) ? f.FACSlope(i, j) : base.Slope(i, j);
        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return Foundation.TryGet(out var f, i, j)
                ? f.FACTileFrame(i, j, ref resetFrame, ref noBreak)
                : base.TileFrame(i, j, ref resetFrame, ref noBreak);
        }
        public override bool UnlockChest(int i, int j, ref short frameXAdjustment, ref int dustType, ref bool manual)
        {
            return Foundation.TryGet(out var f, i, j)
                ? f.FACUnlockChest(i, j, ref frameXAdjustment, ref dustType, ref manual)
                : base.UnlockChest(i, j, ref frameXAdjustment, ref dustType, ref manual);
        }
    }
}
