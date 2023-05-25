using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace FAC
{
    public abstract class FoundationTile<T> : ModTile where T : Foundation
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
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
            //TileObjectData.newTile.HookPostPlaceMyPlayer = tileEntity is not null
            //    ? new PlacementHook(tileEntity.Hook_AfterPlacement, -1, 0, false)
            //    : new PlacementHook(Hook_AfterPlacement_NoEntity, -1, 0, false);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook((i, j, placeType, style, direction, alternate) =>
            {
                ModTileEntity tileEntity = ModContent.GetInstance<T>();
                Main.NewText(typeof(T).FullName);
                if (tileEntity is null)
                {
                    Main.NewText("null");
                    return Hook_AfterPlacement_NoEntity(i, j, placeType, style, direction, alternate);
                }
                Main.NewText("not null");
                return tileEntity.Hook_AfterPlacement(i, j, placeType, style, direction, alternate);
            }, -1, 0, false);
            //TileID.Sets.DisableSmartCursor[Type] = true;
            //TileID.Sets.HasOutlines[Type] = true;
        }
        public int Hook_AfterPlacement_NoEntity(int i, int j, int type, int style, int direction, int alternate)
        {
            Main.NewText("do have not entity");
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
    }
}
