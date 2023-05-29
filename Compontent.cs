using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FAC
{
    public abstract class Compontent : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => mod is not FAC;
        public Foundation Foundation { get; internal set; }
        public virtual bool IsVisible => true;
        public virtual bool IsActive => true;
        public virtual void Update() { }
        public virtual bool CompatibleWith(Compontent other) => true;
        public virtual void OnEquip() { }
        public virtual void Draw() { }
        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            if (Foundation is not null)
            {
                tag[nameof(Foundation)] = Foundation.UID.ToString();
            }
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet(nameof(Foundation), out string uid))
            {
                foreach (TileEntity te in TileEntity.ByID.Values)
                {
                    if (te is Foundation f && f.UID.ToString() == uid)
                    {
                        Foundation = f;
                        break;
                    }
                }
            }
        }
        public virtual bool FACPreDraw(int i, int j, SpriteBatch spriteBatch) => true;
        public virtual void FACPostDraw(int i, int j, SpriteBatch spriteBatch) { }
        public virtual void FACDrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData) { }
        public virtual void FACSetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) { }
        public virtual void FACSpecialDraw(int i, int j, SpriteBatch spriteBatch) { }
        /// <summary>
        /// 任意组件返回false时Foundation返回false
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACCanExplode(int i, int j)
        {
            return true;
        }
        /// <summary>
        /// 任意组件返回false时Foundation返回false
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="blockDamaged"></param>
        /// <returns></returns>
        public virtual bool FACCanKillTile(int i, int j, ref bool blockDamaged)
        {
            return true;
        }
        public virtual void FACAnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
        }
        /// <summary>
        /// 任意组件返回false时Foundation返回false
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool FACAutoSelect(int i, int j, Item item)
        {
            return false;
        }
        /// <summary>
        /// 任意组件返回false时Foundation返回false
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACCanPlace(int i, int j)
        {
            return true;
        }
        /// <summary>
        /// 任意组件返回false时Foundation返回false
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool FACCreateDust(int i, int j, ref int type)
        {
            return true;
        }
        /// <summary>
        /// 任意组件返回false时Foundation返回false
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACDrop(int i, int j)
        {
            return true;
        }
        public virtual void FACDropCritterChance(int i, int j, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance)
        {
        }
        public virtual void FACGetMapOption(int i, int j, ref ushort option)
        {
        }
        /// <summary>
        /// 任意组件返回true时Foundation返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public virtual bool FACHasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return false;
        }
        public virtual void FACHitWire(int i, int j)
        {
        }
        /// <summary>
        /// 任意组件返回true时Foundation返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACIsLockedChest(int i, int j)
        {
            return false;
        }
        /// <summary>
        /// 任意组件返回true时Foundation返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual bool FACIsTileDangerous(int i, int j, Player player)
        {
            return false;
        }
        /// <summary>
        /// 任意组件返回true时Foundation返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACIsTileSpelunkable(int i, int j)
        {
            return false;
        }
        public virtual void FACKillMultiTile(int i, int j, int frameX, int frameY)
        {
        }
        /// <summary>
        /// 任意组件返回true时Foundation返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="fail"></param>
        /// <returns></returns>
        public virtual bool FACKillSound(int i, int j, bool fail)
        {
            return true;
        }
        public virtual void FACKillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
        }
        public virtual void FACModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
        }
        public virtual void FACModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
        }
        public virtual void FACModifySleepingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
        }
        public virtual void FACMouseOverFar(int i, int j)
        {
        }
        public virtual void FACNearbyEffects(int i, int j, bool closer)
        {
        }
        public virtual void FACNumDust(int i, int j, bool fail, ref int num)
        {
        }
        public virtual void FACPlaceInWorld(int i, int j, Item item)
        {
        }
        public virtual void FACRandomUpdate(int i, int j)
        {
        }
        /// <summary>
        /// 任意组件返回true时Foundation返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACRightClick(int i, int j)
        {
            return false;
        }
        public virtual void FACSetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
        }
        /// <summary>
        /// 任意组件返回true时Foundation返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public virtual bool FACSlope(int i, int j)
        {
            return true;
        }
        /// <summary>
        /// 任意组件返回true时Foundation返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="resetFrame"></param>
        /// <param name="noBreak"></param>
        /// <returns></returns>
        public virtual bool FACTileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return true;
        }
        /// <summary>
        /// 任意组件返回true时Foundation返回true
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="frameXAdjustment"></param>
        /// <param name="dustType"></param>
        /// <param name="manual"></param>
        /// <returns></returns>
        public virtual bool FACUnlockChest(int i, int j, ref short frameXAdjustment, ref int dustType, ref bool manual)
        {
            return false;
        }
    }
}
