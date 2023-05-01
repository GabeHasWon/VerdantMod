using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ObjectData;
using Verdant.Bezier;
using Terraria.Utilities;
using System.Collections.Generic;
using System.Linq;
using System;

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

    public static int GenLine(Point start, Point end, int tileType, ref List<Point> points, int cap = -1)
    {
        int repeats = (int)Vector2.Distance(start.ToVector2() + new Vector2(8), end.ToVector2() + new Vector2(8));
        int count = 0;
        Point last = new();

        if (cap == -1)
            cap = repeats + 1;

        for (int i = 0; i <= repeats; ++i)
        {
            if (i > cap)
                break;

            Point placePos = Vector2.Lerp(start.ToVector2(), end.ToVector2(), i / (float)repeats).ToPoint();

            if (WorldGen.SolidTile(placePos.X, placePos.Y) || placePos == last)
                continue;

            if (WorldGen.PlaceTile(placePos.X, placePos.Y, tileType, true, true))
            {
                count++;
                last = placePos;
                points.Add(placePos);
            }
        }

        return count;
    }

    public static int GenLine(Point start, Point end, int tileType, int cap = -1)
    {
        List<Point> throwaway = new();
        return GenLine(start, end, tileType, ref throwaway);
    }

    private static int RecursiveFill(Point originalPos, int x, int y, int type, ref int repeats, int maxRepeats, bool forced = false)
    {
        if (Main.tile[x, y].HasTile || !WorldGen.InWorld(x, y, 4) || repeats > maxRepeats)
            return repeats;

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

        return repeats;
    }

    public static int RecursiveFill(Point point, int type, int repeats, int maxRepeats, bool forced = false) => RecursiveFill(point, point.X, point.Y, type, ref repeats, maxRepeats, forced);

    private static int RecursiveFillGetPoints(Point originalPos, int x, int y, int type, ref int repeats, int maxRepeats, ref List<Point> points, bool forced = false)
    {
        if (Main.tile[x, y].HasTile || !WorldGen.InWorld(x, y, 4) || repeats > maxRepeats)
            return repeats;

        WorldGen.PlaceTile(x, y, type, true, forced);
        repeats++;

        Tile tile = Main.tile[x, y];
        if (!points.Contains(new Point(x, y)) && tile.HasTile && tile.TileType == type)
            points.Add(new Point(x, y));

        if (x >= originalPos.X)
            RecursiveFillGetPoints(originalPos, x + 1, y, type, ref repeats, maxRepeats, ref points);

        if (x <= originalPos.X)
            RecursiveFillGetPoints(originalPos, x - 1, y, type, ref repeats, maxRepeats, ref points);

        if (y >= originalPos.Y)
            RecursiveFillGetPoints(originalPos, x, y + 1, type, ref repeats, maxRepeats, ref points);

        if (y <= originalPos.Y)
            RecursiveFillGetPoints(originalPos, x, y - 1, type, ref repeats, maxRepeats, ref points);

        return repeats;
    }

    public static int RecursiveFillGetPoints(Point point, int type, int repeats, int maxRepeats, ref List<Point> points, bool forced = false) => 
        RecursiveFillGetPoints(point, point.X, point.Y, type, ref repeats, maxRepeats, ref points, forced);

    public static void Ellipse(Action<int, int> action, Point first, Point last, ref List<Point> points)
    {
        Point topLeft = new(Math.Min(first.X, last.X), Math.Min(first.Y, last.Y));
        Point bottomRight = new(Math.Max(first.X, last.X), Math.Max(first.Y, last.Y));
        Vector2 center = Vector2.Lerp(topLeft.ToVector2(), bottomRight.ToVector2(), 0.5f);

        int width = bottomRight.X - topLeft.X;
        int height = bottomRight.Y - topLeft.Y;
        float perimeter = MathHelper.Pi * ((3 * (width + height)) - MathF.Sqrt((3 * width + height) * (width + 3 * height))); //Approximate perimeter

        if (perimeter == 0)
            return;

        Point lastPlace = new();
        float interval = MathHelper.TwoPi / (perimeter * 2);

        for (float repeats = 0; repeats < MathHelper.TwoPi; repeats += interval)
        {
            int x = (int)(width * MathF.Cos(repeats)) + (int)center.X;
            int y = (int)(height * MathF.Sin(repeats)) + (int)center.Y;

            if (Vector2.Distance(lastPlace.ToVector2(), new Vector2(x, y)) >= 1)
            {
                action(x, y);
                lastPlace = new Point(x, y);
                points.Add(lastPlace);
            }
        }
    }

    public static void Ellipse(Action<int, int> action, Point first, Point last)
    {
        List<Point> throwAway = new();
        Ellipse(action, first, last, ref throwAway);
    }
}
