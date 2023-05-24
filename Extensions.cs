using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace FAC
{
    public static class Extensions
    {
        public static void WriteRGBA(this BinaryWriter w, Color color)
        {
            w.Write(color.R);
            w.Write(color.G);
            w.Write(color.B);
            w.Write(color.A);
        }
        public static Color ReadRGBA(this BinaryReader r)
        {
            return new Color(r.ReadByte(), r.ReadByte(), r.ReadByte(), r.ReadByte());
        }
        public static void Write(this BinaryWriter w, Point point)
        {
            w.Write(point.X);
            w.Write(point.Y);
        }
        public static Point ReadPoint(this BinaryReader r)
        {
            return new Point(r.ReadInt32(), r.ReadInt32());
        }
        public static void Write(this BinaryWriter w, Point16 point)
        {
            w.Write(point.X);
            w.Write(point.Y);
        }
        public static Point16 ReadPoint16(this BinaryReader r)
        {
            return new Point16(r.ReadInt16(), r.ReadInt16());
        }
        public static void Write(this BinaryWriter w, Item item, bool writeStack = true, bool writeFavorite = false)
        {
            ItemIO.Send(item ?? new Item(), w, writeStack, writeFavorite);
        }
        public static Item ReadItem(this BinaryReader r, bool readStack = true, bool readFavorite = false)
        {
            return ItemIO.Receive(r, readStack, readFavorite);
        }
        public static void Write(this BinaryWriter w, Item[] items, bool writeStack = true, bool writeFavorite = false)
        {
            w.Write(items.Length);
            for (int i = 0; i < items.Length; i++)
            {
                ItemIO.Send(items[i] ?? new Item(), w, writeStack, writeFavorite);
            }
        }
        public static Item[] ReadItemArray(this BinaryReader r, bool readStack = true, bool readFavorite = false)
        {
            int num = r.ReadInt32();
            Item[] array = new Item[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = ItemIO.Receive(r, readStack, readFavorite);
            }

            return array;
        }
        public static Point16 GetTileOrigin(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            TileObjectData tileData = TileObjectData.GetTileData(tile.TileType, 0);
            if (tileData == null)
            {
                return Point16.NegativeOne;
            }
            int frameX = tile.TileFrameX;
            int frameY = tile.TileFrameY;
            int subX = frameX % tileData.CoordinateFullWidth;
            int subY = frameY % tileData.CoordinateFullHeight;
            Point16 coord = new(i, j);
            Point16 frame = new(subX / 18, subY / 18);
            return coord - frame;
        }
        public static bool TryGetTileEntityAs<T>(int i, int j, out T entity) where T : TileEntity
        {
            Point16 origin = GetTileOrigin(i, j);
            if (TileEntity.ByPosition.TryGetValue(origin, out TileEntity existing) && existing is T existingAsT)
            {
                entity = existingAsT;
                return true;
            }
            entity = null;
            return false;
        }
    }
}
