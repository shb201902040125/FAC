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
        public abstract int ItemType { get; }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ItemType;
            player.noThrow = 2;
        }
        public void SetFACStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.LavaDeath = false;
            ModTileEntity tileEntity = ModContent.GetInstance<T>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = tileEntity is not null
                ? new PlacementHook(tileEntity.Hook_AfterPlacement, -1, 0, false)
                : new PlacementHook(Hook_AfterPlacement_NoEntity, -1, 0, false);
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
        }
        public int Hook_AfterPlacement_NoEntity(int i, int j, int type, int style, int direction, int alternate)
        {
            TileObjectData data = TileObjectData.GetTileData(type, style, alternate);
            if (data is null)
            {
                throw new Exception("This foundation needs to be bound to ModTiles which have TileObjectData registered");
            }
            var point = Extensions.GetTileOrigin(i, j);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, point.X, point.Y, data.Width, data.Height);
            }
            return 0;
        }
    }
}
