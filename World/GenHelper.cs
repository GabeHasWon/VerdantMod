using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ObjectData;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Runtime.CompilerServices;
using Verdant.World.Utility;

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

    public static void ReplaceWall(Point16 p, int t, bool mute = true)
    {
        WorldGen.KillWall(p.X, p.Y, false);
        WorldGen.PlaceWall(p.X, p.Y, t, mute);
    }

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

    public static void ActOnBezier(float distance, Action<int, int> perPointAction, params Vector2[] controlPoints)
    {
        Bezier bezier = new(distance, controlPoints);

        foreach (Vector2 point in bezier.GeneratedPoints)
            perPointAction((int)point.X, (int)point.Y);
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
        if (start == end)
        {
            if (WorldGen.PlaceTile(start.X, start.Y, tileType, true, true))
                return 1;
            
            return 0;
        }

        Vector2 startVec = start.ToVector2();
        Vector2 endVec = end.ToVector2();
        int repeats = (int)Vector2.Distance(startVec, endVec);
        int count = 0;
        Point last = new();
        bool hasPoints = !Unsafe.IsNullRef(in points);

        if (cap == -1)
            cap = repeats + 1;

        for (int i = 0; i <= repeats; ++i)
        {
            if (i > cap)
                break;

            Point placePos = Vector2.Lerp(startVec + new Vector2(0.5f), endVec + new Vector2(0.5f), i / (float)repeats).ToPoint();

            if (WorldGen.SolidTile(placePos.X, placePos.Y) || placePos == last)
                continue;

            if (WorldGen.PlaceTile(placePos.X, placePos.Y, tileType, true, true))
            {
                count++;
                last = placePos;

                if (hasPoints)
                    points.Add(placePos);
            }
        }

        return count;
    }

    public static int GenLine(Point start, Point end, int tileType) => GenLine(start, end, tileType, ref Unsafe.NullRef<List<Point>>());

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
        bool cantPlace = (!forced && Main.tile[x, y].HasTile) || (forced && WorldGen.SolidOrSlopedTile(x, y));
        if (cantPlace || !WorldGen.InWorld(x, y, 4) || repeats > maxRepeats)
            return repeats;

        WorldGen.PlaceTile(x, y, type, true, true);
        repeats++;

        Tile tile = Main.tile[x, y];
        if (!points.Contains(new Point(x, y)) && tile.HasTile && tile.TileType == type)
            points.Add(new Point(x, y));

        if (x >= originalPos.X)
            RecursiveFillGetPoints(originalPos, x + 1, y, type, ref repeats, maxRepeats, ref points, forced);

        if (x <= originalPos.X)
            RecursiveFillGetPoints(originalPos, x - 1, y, type, ref repeats, maxRepeats, ref points, forced);

        if (y >= originalPos.Y)
            RecursiveFillGetPoints(originalPos, x, y + 1, type, ref repeats, maxRepeats, ref points, forced);

        if (y <= originalPos.Y)
            RecursiveFillGetPoints(originalPos, x, y - 1, type, ref repeats, maxRepeats, ref points, forced);

        return repeats;
    }

    public static int RecursiveFillGetPoints(Point point, int type, int repeats, int maxRepeats, ref List<Point> points, bool forced = false) => 
        RecursiveFillGetPoints(point, point.X, point.Y, type, ref repeats, maxRepeats, ref points, forced);

    public static void Ellipse(Action<int, int> action, Point first, Point last, ref List<Point> points)
    {
        Point topLeft = new(Math.Min(first.X, last.X), Math.Min(first.Y, last.Y));
        Point bottomRight = new(Math.Max(first.X, last.X), Math.Max(first.Y, last.Y));
        Vector2 center = Vector2.Lerp(topLeft.ToVector2(), bottomRight.ToVector2(), 0.5f);

        bool hasPoints = !Unsafe.IsNullRef(in points);
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

            if (Vector2.DistanceSquared(lastPlace.ToVector2(), new Vector2(x, y)) >= 1)
            {
                action(x, y);
                lastPlace = new Point(x, y);

                if (hasPoints)
                    points.Add(lastPlace);
            }
        }
    }

    public static void Ellipse(Action<int, int> action, Point first, Point last) => Ellipse(action, first, last, ref Unsafe.NullRef<List<Point>>());
}
