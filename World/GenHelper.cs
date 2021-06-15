using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.WorldGen;
using static Terraria.ModLoader.ModContent;
using Verdant.Tiles.Verdant.Basic;
using Terraria.ObjectData;
using System;
using Verdant.Bezier;
using Terraria.Utilities;

namespace Verdant.World
{
    public static class GenHelper
    {
        /// <summary>Kills, then replaces, a tile.</summary>
        /// <param name="i">X position.</param>
        /// <param name="j">Y position.</param>
        /// <param name="t">Type to replace with.</param>
        /// <param name="item">Does it drop an item?</param>
        /// <param name="mute">Is it muted?</param>
        /// <param name="style">Style to use when placing.</param>
        public static void ReplaceTile(int i, int j, int t, bool item = false, bool mute = true, int style = 0)
        {
            KillTile(i, j, false, false, item);
            PlaceTile(i, j, t, true, mute, -1, style);
        }
        public static void ReplaceTile(Point p, int t, bool item = false, bool mute = true, int style = 0) => ReplaceTile(p.X, p.Y, t, item, mute, style);
        public static void ReplaceTile(Point16 p, int t, bool item = false, bool mute = true, int style = 0) => ReplaceTile(p.X, p.Y, t, item, mute, style);

        public static void ReplaceWall(Point p, int t, bool mute = true)
        {
            KillWall(p.X, p.Y, false);
            PlaceWall(p.X, p.Y, t, mute);
        }
        public static void ReplaceWall(int x, int y, int t, bool mute = true) => ReplaceWall(new Point(x, y), t, mute);

        public static void GenLine(Vector2 p, Vector2 rot, Vector2 o, int lW, int hW, int targetType = TileID.Dirt, int type = TileID.Dirt)
        {
            while (!Framing.GetTileSafely((int)p.X, (int)p.Y).active() || !Main.tileSolid[Framing.GetTileSafely((int)p.X, (int)p.Y).type] || Framing.GetTileSafely((int)p.X, (int)p.Y).type == targetType)
            {
                for (int j = lW; j < hW; ++j)
                {
                    Vector2 offset = rot.RotatedBy(1.571f);
                    KillTile((int)p.X - ((int)offset.X * j), (int)p.Y - ((int)offset.Y * j), false, false, true);
                    PlaceTile((int)p.X - ((int)offset.X * j), (int)p.Y - ((int)offset.Y * j), type, true, false, -1);
                }
                p = new Vector2(p.X + o.X, p.Y + o.Y);
            }
        }

        public static void PlaceMultitile(Point position, int type, int style = -1)
        {
            TileObjectData data = TileObjectData.GetTileData(type, style); //Get data

            if ((position.X + data.Width > Main.maxTilesX || position.X < 0) || (position.Y + data.Height > Main.maxTilesY || position.Y < 0)) return; //Check position

            int xVar = 0;
            int yVar = 0;
            if (data.StyleHorizontal) xVar = (style == -1 ? Main.rand.Next(data.RandomStyleRange) : style);
            else yVar = (style == -1 ? Main.rand.Next(data.RandomStyleRange) : style);

            for (int x = 0; x < data.Width; x++) //Column
            {
                for (int y = 0; y < data.Height; y++) //Row
                {
                    Tile tile = Framing.GetTileSafely(position.X + x, position.Y + y); //get the targeted tile
                    tile.type = (ushort)type; //set the type of the tile to our multitile

                    tile.frameX = (short)((x + (data.Width * xVar)) * (data.CoordinateWidth + data.CoordinatePadding)); //set the X frame appropriately
                    if (data.CoordinateHeights[y] != 18)
                        tile.frameY = (short)((y + (data.Height * yVar)) * (data.CoordinateHeights[y] + data.CoordinatePadding)); //set the Y frame appropriately
                    else
                        tile.frameY = (short)((y + (data.Height * yVar)) * (data.CoordinateHeights[y])); //set the Y frame appropriately
                    tile.active(true); //activate the tile
                }
            }
        }

        public static void BasicBox(int x, int y, int width, int height, int type, int wallType = WallID.GrassUnsafe)
        {
            for (int i = -width / 2; i < width / 2; ++i)
            {
                int nX = x + i;
                ReplaceTile(nX, y - (height / 2), type, false, true, 0);
                ReplaceTile(nX, y + (height / 2), type, false, true, 0);
            }

            for (int j = -height / 2; j < height / 2 + 1; ++j)
            {
                int nY = y + j;
                ReplaceTile(x - (width / 2), nY, type, false, true, 0);
                ReplaceTile(x + (width / 2), nY, type, false, true, 0);
            }

            for (int i = -width / 2 + 1; i < width / 2; ++i)
            {
                for (int j = -height / 2 + 1; j < height / 2; ++j)
                {
                    ReplaceWall(x + i, y + j, wallType, true);
                }
            }
        }

        public static void BasicBox(Point pos, Point size, int type, int wallType) => BasicBox(pos.X, pos.Y, size.X, size.Y, type, wallType);
        public static void BasicBox(Point pos, int width, int height, int type, int wallType) => BasicBox(pos.X, pos.Y, width, height, type, wallType);
        public static void BasicBox(int x, int y, Point size, int type, int wallType) => BasicBox(x, y, size.X, size.Y, type, wallType);

        /// <summary>Places a chest with items in it.</summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        /// <param name="type">Type if the chest.</param>
        /// <param name="mainItems">List of "main" items, like the main weapon or tool.</param>
        /// <param name="subItems">List of "sub" items - filler, basically - like potions, weak, stackable weapons, or materials.</param>
        /// <param name="stackFill">If true, stacks an item of type A on pre-existing stack of A. Otherwise, do not repeat fill items.</param>
        /// <param name="r">Use Main.rand for in-game generation, use WorldGen.genRand for worldgen.</param>
        /// <param name="subItemLength">How many sub item stacks there are.</param>
        /// <param name="style">Style of the chest.</param>
        public static bool PlaceChest(int x, int y, int type, (int, int)[] mainItems, (int, int)[] subItems, bool stackFill = true, UnifiedRandom r = null, int subItemLength = 6, int style = 0, bool overRide = false)
        {
            r = r ?? Main.rand;

            if (overRide)
            {
                KillTile(x, y, false, false, true);
                KillTile(x + 1, y, false, false, true);
                KillTile(x, y + 1, false, false, true);
                KillTile(x + 1, y + 1, false, false, true);
            }

            int ChestIndex = WorldGen.PlaceChest(x, y, (ushort)type, false, style);
            if (ChestIndex != -1)
            {
                int main = r.Next(mainItems.Length);
                Main.chest[ChestIndex].item[0].SetDefaults(mainItems[main].Item1);
                Main.chest[ChestIndex].item[0].stack = mainItems[main].Item2;

                int reps = 0;

                for (int i = 0; i < subItemLength; ++i)
                {
                repeat:
                    if (reps > 50)
                    {
                        VerdantMod.Instance.Logger.Info("WARNING: Attempted to repeat stack too often. Report to dev.");
                        break;  
                    }

                    int sub = r.Next(subItems.Length);
                    for (int j = 1; j < Main.chest[ChestIndex].item.Length; ++j)
                    {
                        if (Main.chest[ChestIndex].item[j].type == subItems[sub].Item1)
                        {
                            if (stackFill)
                            {
                                Main.chest[ChestIndex].item[j].stack += subItems[sub].Item2;
                                i++;
                            }
                            reps++;
                            goto repeat; //imagine using goto LOL
                        }
                    }

                    Main.chest[ChestIndex].item[i + 1].SetDefaults(subItems[sub].Item1);
                    Main.chest[ChestIndex].item[i + 1].stack = subItems[sub].Item2;
                }
                return true;
            }
            return false;
        }

        /// <summary>Places a chest with items in it.</summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        /// <param name="type">Tile ID of the chest.</param>
        /// <param name="style">Style for the chest.</param>
        /// <param name="items">Items, in order.</param>
        public static bool PlaceChest(int x, int y, int type, int style, bool overRide, params (int, int)[] items)
        {
            if (overRide)
            {
                KillTile(x, y, false, false, true);
                KillTile(x + 1, y, false, false, true);
                KillTile(x, y + 1, false, false, true);
                KillTile(x + 1, y + 1, false, false, true);
            }

            int ChestIndex = WorldGen.PlaceChest(x, y, (ushort)type, false, style);
            if (ChestIndex != -1)
            {
                for (int i = 0; i < items.Length; ++i)
                {
                    Main.chest[ChestIndex].item[i].SetDefaults(items[i].Item1);
                    Main.chest[ChestIndex].item[i].stack = items[i].Item2;
                }
                return true;
            }
            return false;
        }

        public static Point[] GetBezier(double[] orderedPositions, int fidelity = 30)
        {
            const int POINTS_ON_CURVE = 100;

            BezierCurve curve = new BezierCurve();
            double[] p = new double[POINTS_ON_CURVE];

            curve.Bezier2D(orderedPositions, POINTS_ON_CURVE / 2, p);

            Point[] res = new Point[POINTS_ON_CURVE];
            for (int i = 1; i != POINTS_ON_CURVE - 1; i += 2)
                res[i] = new Point((int)p[i + 1], (int)p[i]);
            return res;
        }

        public static void GenBezierDirect(double[] orderedPositions, int fidelity = 100, int t = TileID.Dirt, bool overRide = false, int height = 1)
        {
            int POINTS_ON_CURVE = fidelity;

            BezierCurve curve = new BezierCurve();
            double[] p = new double[POINTS_ON_CURVE];

            curve.Bezier2D(orderedPositions, POINTS_ON_CURVE / 2, p);

            for (int i = 1; i != POINTS_ON_CURVE - 1; i += 2)
            {
                for (int j = 0; j < height; ++j)
                {
                    if (overRide)
                        ReplaceTile((int)p[i + 1], (int)p[i] - j, t, false, true);
                    else if (!overRide && !Framing.GetTileSafely((int)p[i + 1], (int)p[i]).active())
                        PlaceTile((int)p[i + 1], (int)p[i] - j, t, true, false);
                }
            }
        }

        public static void GenBezierDirectWall(double[] orderedPositions, int fidelity = 100, int t = WallID.Stone, bool overRide = false, int height = 1)
        {
            int POINTS_ON_CURVE = fidelity;

            BezierCurve curve = new BezierCurve();
            double[] p = new double[POINTS_ON_CURVE];

            curve.Bezier2D(orderedPositions, POINTS_ON_CURVE / 2, p);

            for (int i = 1; i != POINTS_ON_CURVE - 1; i += 2)
            {
                for (int j = 0; j < height; ++j)
                {
                    if (overRide)
                        ReplaceWall((int)p[i + 1], (int)p[i] - j, t, true);
                    else if (!overRide && !Framing.GetTileSafely((int)p[i + 1], (int)p[i]).active())
                        PlaceWall((int)p[i + 1], (int)p[i] - j, t, true);
                }
            }
        }

        public static void GenBezierDirect(Point[] orderedPositions, int fidelity = 100, int t = TileID.Dirt)
        {
            double[] realPos = new double[orderedPositions.Length * 2];
            for (int i = 0; i < orderedPositions.Length; ++i)
            {
                realPos[i] = orderedPositions[i].X;
                realPos[i + 1] = orderedPositions[i].Y;
            }
            GenBezierDirect(realPos, fidelity, t);
        }

        /// <summary>Creates a vine-like bezier curve.</summary>
        /// <param name="width">X distance between the first point and the last point.</param>
        /// <param name="heightDifference">Y distance between the first point and the last point.</param>
        /// <param name="dip">How far down it dips.</param>
        public static void GenBezier(int width, int heightDifference, int type = TileID.Grass, int dip = 15)
        {
            int height = Main.rand.Next(10, 20);
            int heightRand = Main.rand.Next(-7, 8);
            GenBezierDirect(new Point[] {
                Helper.MouseTile(),
                new Point(Helper.MouseTile().X + (width / 2), Helper.MouseTile().Y + height),
                new Point(Helper.MouseTile().X + width, Helper.MouseTile().Y + heightRand) }, 30, type);
        }

        public static void KillRectangle(int x, int y, int w, int h)
        {
            for (int i = x; i < x + w; ++i)
                for (int j = y; j < y + h; ++j)
                    KillTile(i, j);
        }

        public static void Place(int x, int y, int type, int style = -1)
        {
            Tile t = Framing.GetTileSafely(x, y);
            t.type = (ushort)type;
            t.active(true);
            if (style == -1)
                SquareTileFrame(x, y, true);
            else
                t.frameX = (short)(18 * style);
        }
    }
}
