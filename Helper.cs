using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using static Terraria.WorldGen;

namespace Verdant
{
    public class Helper
    {
        public static int AnyTileRectangle(int i, int j, int w, int h)
        {
            int count = 0;
            for (int l = i; l < i + w; ++l)
                for (int k = j; k < j + h; ++k)
                    if (Framing.GetTileSafely(l, k).active())
                        count++;
            return count;
        }

        public static int NoTileRectangle(int i, int j, int w, int h)
        {
            int count = 0;
            for (int l = i; l < i + w; ++l)
                for (int k = j; k < j + h; ++k)
                    if (!Framing.GetTileSafely(l, k).active())
                        count++;
            return count;
        }

        public static int TileRectangle(int i, int j, int w, int h, params int[] types)
        {
            int count = 0;
            for (int l = i; l < i + w; ++l)
                for (int k = j; k < j + h; ++k)
                    if (Framing.GetTileSafely(l, k).active() && types.Any(x => x == Framing.GetTileSafely(l, k).type))
                        count++;
            return count;
        }

        public static bool AreaClear(int i, int j, int w, int h) => AnyTileRectangle(i, j, w, h) == 0;

        public static int WallRectangle(int i, int j, int w, int h)
        {
            int count = 0;
            for (int l = i; l < i + w; ++l)
                for (int k = j; k < j + h; ++k)
                    if (Main.tile[l, k].wall != WallID.None)
                        count++;
            return count;
        }

        public static bool WalledSquare(int i, int j, int w, int h) => WallRectangle(i, j, w, h) == w * h;

        public static int WalledType(int i, int j, int w, int h, int type)
        {
            int count = 0;
            for (int l = i; l < i + w; ++l)
                for (int k = j; k < j + h; ++k)
                    if (Main.tile[l, k].wall != WallID.None && Main.tile[l, k].wall == type)
                        count++;
            return count;
        }

        public static bool WalledSquareType(int i, int j, int w, int h, int type) => WallRectangle(i, j, w, h) == w * h && WalledType(i, j, w, h, type) == w * h;

        public static int FindDown(Vector2 worldPos)
        {
            Point tPos = (worldPos / 16).ToPoint();
            while (!Main.tile[tPos.X, tPos.Y].active() || !Main.tileSolid[Main.tile[tPos.X, tPos.Y].type])
                tPos.Y++;
            return tPos.Y;
        }

        public static Point MouseTile() => (Main.MouseWorld / 16f).ToPoint();
        public static Point MouseTile(Point offset) => ((Main.MouseWorld / 16f) + offset.ToVector2()).ToPoint();
        public static Point MouseTile(Vector2 offset) => ((Main.MouseWorld / 16f) + offset).ToPoint();
    }
}
