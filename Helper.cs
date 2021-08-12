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
        public static Vector2 TileOffset => Lighting.lightMode > 1 ? Vector2.Zero : Vector2.One * 12;

        public static Vector2 TileCustomPosition(int i, int j, Vector2? off = null)
        {
            return ((new Vector2(i, j) + TileOffset) * 16) - Main.screenPosition - (off ?? new Vector2(0));
        }

        public static bool HasOpenAdjacent(int i, int j)
        {
            for (int l = -1; l < 1; ++l)
            {
                for (int k = -1; k < 1; ++k)
                {
                    if (new Point(i + l, j + k) != new Point(i, j) && !Framing.GetTileSafely(i + l, j + k).active())
                        return true;
                }
            }
            return false;
        }

        public static Point GetOpenAdjacent(int i, int j)
        {
            for (int l = -1; l < 1; ++l)
            {
                for (int k = -1; k < 1; ++k)
                {
                    if (!Framing.GetTileSafely(i + l, j + k).active() && new Point(i + l, j + k) != new Point(i, j))
                        return new Point(l, k);
                }
            }
            return new Point(-2, -2);
        }

        public static Point GetRandomOpenAdjacent(int i, int j)
        {
            List<Point> adjacents = new List<Point>();
            for (int l = -1; l < 1; ++l)
            {
                for (int k = -1; k < 1; ++k)
                {
                    if (!Framing.GetTileSafely(i + l, j + k).active() && new Point(i + l, j + k) != new Point(i, j))
                        adjacents.Add(new Point(l, k));
                }
            }
            if (adjacents.Count > 0)
                return adjacents[genRand.Next(adjacents.Count)];
            return new Point(-2, -2);
        }

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
            {
                for (int k = j; k < j + h; ++k)
                {
                    if (Framing.GetTileSafely(l, k).active() && types.Any(x => x == Framing.GetTileSafely(l, k).type))
                        count++;
                }
            }
            return count;
        }

        public static bool AreaClear(int i, int j, int w, int h)
        {
            return AnyTileRectangle(i, j, w, h) == 0;
        }

        public static int WallRectangle(int i, int j, int w, int h)
        {
            int count = 0;
            for (int l = i; l < i + w; ++l)
            {
                for (int k = j; k < j + h; ++k)
                {
                    if (Main.tile[l, k].wall != WallID.None)
                        count++;
                }
            }
            return count;
        }

        public static bool WalledSquare(int i, int j, int w, int h)
        {
            return WallRectangle(i, j, w, h) == w * h;
        }

        public static int WalledType(int i, int j, int w, int h, int type)
        {
            int count = 0;
            for (int l = i; l < i + w; ++l)
            {
                for (int k = j; k < j + h; ++k)
                {
                    if (Main.tile[l, k].wall != WallID.None && Main.tile[l, k].wall == type)
                        count++;
                }
            }
            return count;
        }

        public static bool WalledSquareType(int i, int j, int w, int h, int type)
        {
            return WallRectangle(i, j, w, h) == w * h && WalledType(i, j, w, h, type) == w * h;
        }

        public static int FindDown(Vector2 worldPos)
        {
            Point tPos = (worldPos / 16).ToPoint();
            while (!Main.tile[tPos.X, tPos.Y].active() || !Main.tileSolid[Main.tile[tPos.X, tPos.Y].type])
            {
                tPos.Y++;
            }
            return tPos.Y;
        }

        public static bool SolidTile(int i, int j) => Framing.GetTileSafely(i, j).active() && Main.tileSolid[Framing.GetTileSafely(i, j).type];
        public static bool SolidTopTile(int i, int j) => Framing.GetTileSafely(i, j).active() && (Main.tileSolidTop[Framing.GetTileSafely(i, j).type] || Main.tileSolid[Framing.GetTileSafely(i, j).type]);
        public static bool ActiveType(int i, int j, int t) => Framing.GetTileSafely(i, j).active() && Framing.GetTileSafely(i, j).type == t;
        public static bool SolidType(int i, int j, int t) => ActiveType(i, j, t) && Framing.GetTileSafely(i, j).active();
        public static bool ActiveTypeNoTopSlope(int i, int j, int t) => Framing.GetTileSafely(i, j).active() && Framing.GetTileSafely(i, j).type == t && !Framing.GetTileSafely(i, j).topSlope();

        public static Point MouseTile() => (Main.MouseWorld / 16f).ToPoint();
        public static Point MouseTile(Point offset) => ((Main.MouseWorld / 16f) + offset.ToVector2()).ToPoint();
        public static Point MouseTile(Vector2 offset) => ((Main.MouseWorld / 16f) + offset).ToPoint();
    }
}
