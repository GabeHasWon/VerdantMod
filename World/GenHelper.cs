using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ObjectData;
using Verdant.Bezier;
using Terraria.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Verdant.World;

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
        WorldGen.KillTile(i, j, false, false, item);
        WorldGen.PlaceTile(i, j, t, true, mute, -1, style);
    }

    public static void ReplaceTile(Point p, int t, bool item = false, bool mute = true, int style = 0) => ReplaceTile(p.X, p.Y, t, item, mute, style);
    public static void ReplaceTile(Point16 p, int t, bool item = false, bool mute = true, int style = 0) => ReplaceTile(p.X, p.Y, t, item, mute, style);

    public static void ReplaceWall(Point16 p, int t, bool mute = true)
    {
        WorldGen.KillWall(p.X, p.Y, false);
        WorldGen.PlaceWall(p.X, p.Y, t, mute);
    }

    public static void ReplaceWall(int x, int y, int t, bool mute = true) => ReplaceWall(new Point(x, y), t, mute);
    public static void ReplaceWall(Point p, int t, bool mute = true) => ReplaceWall(new Point16(p.X, p.Y), t, mute);

    public static void GenLine(Vector2 p, Vector2 rot, Vector2 o, int lW, int hW, int targetType = TileID.Dirt, int type = TileID.Dirt)
    {
        while (!Framing.GetTileSafely((int)p.X, (int)p.Y).HasTile || !Main.tileSolid[Framing.GetTileSafely((int)p.X, (int)p.Y).TileType] || 
            Framing.GetTileSafely((int)p.X, (int)p.Y).TileType == targetType)
        {
            for (int j = lW; j < hW; ++j)
            {
                Vector2 offset = rot.RotatedBy(1.571f);
                WorldGen.KillTile((int)p.X - ((int)offset.X * j), (int)p.Y - ((int)offset.Y * j), false, false, true);
                WorldGen.PlaceTile((int)p.X - ((int)offset.X * j), (int)p.Y - ((int)offset.Y * j), type, true, false, -1);
            }
            p = new Vector2(p.X + o.X, p.Y + o.Y);
        }
    }

    public static void PlaceMultitile(Point position, int type, int style = -1)
    {
        TileObjectData data = TileObjectData.GetTileData(type, style); //Get data

        if (position.X + data.Width > Main.maxTilesX || position.X < 0 || position.Y + data.Height > Main.maxTilesY || position.Y < 0) return; //Check position

        int xVar = 0;
        int yVar = 0;
        if (data.StyleHorizontal) xVar = (style == -1 ? Main.rand.Next(data.RandomStyleRange) : style);
        else yVar = (style == -1 ? Main.rand.Next(data.RandomStyleRange) : style);

        for (int x = 0; x < data.Width; x++) //Column
        {
            for (int y = 0; y < data.Height; y++) //Row
            {
                Tile tile = Framing.GetTileSafely(position.X + x, position.Y + y); //get the targeted tile

                tile.TileType = (ushort)type; //set the type of the tile to our multitile

                tile.TileFrameX = (short)((x + (data.Width * xVar)) * (data.CoordinateWidth + data.CoordinatePadding)); //set the X frame appropriately
                if (data.CoordinateHeights[y] != 18)
                    tile.TileFrameY = (short)((y + (data.Height * yVar)) * (data.CoordinateHeights[y] + data.CoordinatePadding)); //set the Y frame appropriately
                else
                    tile.TileFrameY = (short)((y + (data.Height * yVar)) * (data.CoordinateHeights[y])); //set the Y frame appropriately
                tile.HasTile = true; //activate the tile
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

    public static bool FillChest(int x, int y, (int, int)[] mainItems, (int, int)[] subItems, bool noTypeRepeat = true, UnifiedRandom r = null, int subItemLength = 6)
    {
        r ??= Main.rand;

        int ChestIndex = Chest.FindChest(x, y);
        if (ChestIndex != -1)
        {
            int main = r.Next(mainItems.Length);
            Main.chest[ChestIndex].item[0].SetDefaults(mainItems[main].Item1);
            Main.chest[ChestIndex].item[0].stack = mainItems[main].Item2;

            int reps = 0;

            List<int> usedTypes = new List<int>();

            for (int i = 0; i < subItemLength; ++i)
            {
            repeat:
                if (reps > 50)
                {
                    VerdantMod.Instance.Logger.Info("WARNING: Attempted to repeat item placement too often. Report to dev.");
                    break;
                }

                int sub = r.Next(subItems.Length);
                int itemType = subItems[sub].Item1;
                int itemStack = subItems[sub].Item2;

                if (noTypeRepeat && usedTypes.Contains(itemType))
                {
                    reps++;
                    goto repeat;
                }

                usedTypes.Add(itemType);

                Main.chest[ChestIndex].item[i + 1].SetDefaults(itemType);
                Main.chest[ChestIndex].item[i + 1].stack = itemStack;
            }
            return true;
        }
        return false;
    }

    public static void FillChestDirect(int x, int y, params (int, int)[] items)
    {
        int ChestIndex = Chest.FindChest(x, y);
        if (ChestIndex != -1)
        {
            for (int i = 0; i < items.Length; ++i)
            {
                Main.chest[ChestIndex].item[i].SetDefaults(items[i].Item1);
                Main.chest[ChestIndex].item[i].stack = items[i].Item2;
            }
        }
    }

    /// <summary>Places a chest with items in it.</summary>
    /// <param name="x">X position.</param>
    /// <param name="y">Y position.</param>
    /// <param name="type">Type if the chest.</param>
    /// <param name="mainItems">List of "main" items, like the main weapon or tool.</param>
    /// <param name="subItems">List of "sub" items - filler, basically - like potions, weak, stackable weapons, or materials.</param>
    /// <param name="noTypeRepeat">If true, two stacks of the same item will not be placed in a chest..</param>
    /// <param name="r">Use Main.rand for in-game generation, use WorldGen.genRand for worldgen.</param>
    /// <param name="subItemLength">How many sub item stacks there are.</param>
    /// <param name="style">Style of the chest.</param>
    public static bool PlaceChest(int x, int y, int type, (int, int)[] mainItems, (int, int)[] subItems, 
        bool noTypeRepeat = true, UnifiedRandom r = null, int subItemLength = 6, int style = 0, bool overRide = false)
    {
        r ??= Main.rand;

        if (overRide)
        {
            WorldGen.KillTile(x, y, false, false, true);
            WorldGen.KillTile(x + 1, y, false, false, true);
            WorldGen.KillTile(x, y + 1, false, false, true);
            WorldGen.KillTile(x + 1, y + 1, false, false, true);
        }

        int ChestIndex = WorldGen.PlaceChest(x, y, (ushort)type, false, style);
        if (ChestIndex != -1)
        {
            int main = r.Next(mainItems.Length);
            Main.chest[ChestIndex].item[0].SetDefaults(mainItems[main].Item1);
            Main.chest[ChestIndex].item[0].stack = mainItems[main].Item2;

            int reps = 0;

            List<int> usedTypes = new List<int>();

            for (int i = 0; i < subItemLength; ++i)
            {
            repeat:
                if (reps > 50)
                {
                    VerdantMod.Instance.Logger.Info("WARNING: Attempted to repeat item placement too often. Report to dev.");
                    break;  
                }

                int sub = r.Next(subItems.Length);
                int itemType = subItems[sub].Item1;
                int itemStack = subItems[sub].Item2;

                if (noTypeRepeat && usedTypes.Contains(itemType))
                {
                    reps++;
                    goto repeat;
                }

                usedTypes.Add(itemType);

                Main.chest[ChestIndex].item[i + 1].SetDefaults(itemType);
                Main.chest[ChestIndex].item[i + 1].stack = itemStack;
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
            WorldGen.KillTile(x, y, false, false, true);
            WorldGen.KillTile(x + 1, y, false, false, true);
            WorldGen.KillTile(x, y + 1, false, false, true);
            WorldGen.KillTile(x + 1, y + 1, false, false, true);
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

    public static Point[] GetBezier(double[] orderedPositions)
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
                else if (!overRide && !Framing.GetTileSafely((int)p[i + 1], (int)p[i]).HasTile)
                    WorldGen.PlaceTile((int)p[i + 1], (int)p[i] - j, t, true, false);
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
                else if (!overRide && !Framing.GetTileSafely((int)p[i + 1], (int)p[i]).HasTile)
                    WorldGen.PlaceWall((int)p[i + 1], (int)p[i] - j, t, true);
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
    public static void GenBezier(Point start, Point middle, Point end, int width, int type = TileID.Grass)
    {
        GenBezierDirect(new Point[] {
            start,
            middle,
            end }, 30, type);
    }

    public static void KillRectangle(int x, int y, int w, int h)
    {
        for (int i = x; i < x + w; ++i)
            for (int j = y; j < y + h; ++j)
                WorldGen.KillTile(i, j);
    }

    public static void Place(int x, int y, int type, int style = -1)
    {
        Tile t = Framing.GetTileSafely(x, y);
        t.TileType = (ushort)type;
        t.HasTile = true;
        if (style == -1)
            WorldGen.SquareTileFrame(x, y, true);
        else
            t.TileFrameX = (short)(18 * style);
    }

    public static bool CanGrowVerdantTree(int i, int j, int minHeight, params int[] ignoreTypes)
    {
        for (int k = j; k > j - minHeight; k--)
        {
            Tile t = Framing.GetTileSafely(i, k);
            if (ignoreTypes.Contains(t.TileType))
                continue;
            if (t.HasTile)
                return false;
        }
        return true;
    }

    public static void GenLine(Point start, Point end, int tileType)
    {
        int repeats = (int)Vector2.Distance(start.ToVector2(), end.ToVector2());

        for (int i = 0; i <= repeats; ++i)
        {
            Point placePos = Vector2.Lerp(start.ToVector2(), end.ToVector2(), i / (float)repeats).ToPoint();
            WorldGen.PlaceTile(placePos.X, placePos.Y, tileType, true, true);
        }
    }

    private static void RecursiveFill(Point originalPos, int x, int y, int type, ref int repeats, int maxRepeats, bool forced = false)
    {
        if (Main.tile[x, y].HasTile || !WorldGen.InWorld(x, y, 4) || repeats > maxRepeats)
            return;

        WorldGen.PlaceTile(x, y, type, true, forced);
        repeats++;

        if (x >= originalPos.X)
            RecursiveFill(originalPos, x + 1, y, type, ref repeats, maxRepeats);

        if (x <= originalPos.X)
            RecursiveFill(originalPos, x - 1, y, type, ref repeats, maxRepeats);

        if (y >= originalPos.Y)
            RecursiveFill(originalPos, x, y + 1, type, ref repeats, maxRepeats);

        if (y <= originalPos.Y)
            RecursiveFill(originalPos, x, y - 1, type, ref repeats, maxRepeats);
    }

    public static void RecursiveFill(Point point, int type, int repeats, int maxRepeats, bool forced = false) => RecursiveFill(point, point.X, point.Y, type, ref repeats, maxRepeats, forced);
}
