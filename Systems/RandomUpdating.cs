using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Systems;

internal class RandomUpdating : ILoadable
{
    public delegate void RandomUpdateDelegate(int x, int y, bool checkNPCSpawns, int wallDist);

    public static RandomUpdating Self => ModContent.GetInstance<RandomUpdating>();

    private RandomUpdateDelegate SurfaceRandomUpdate;
    private RandomUpdateDelegate UndergroundRandomUpdate;

    public void Load(Mod mod)
    {
        var surfaceRandomUpdateInfo = typeof(WorldGen).GetMethod("UpdateWorld_OvergroundTile", BindingFlags.Static | BindingFlags.NonPublic);
        var undergroundRandomUpdateInfo = typeof(WorldGen).GetMethod("UpdateWorld_UndergroundTile", BindingFlags.Static | BindingFlags.NonPublic);

        SurfaceRandomUpdate = Delegate.CreateDelegate(typeof(RandomUpdateDelegate), surfaceRandomUpdateInfo) as RandomUpdateDelegate;
        UndergroundRandomUpdate = Delegate.CreateDelegate(typeof(RandomUpdateDelegate), undergroundRandomUpdateInfo) as RandomUpdateDelegate;
    }

    public void Unload() { }

    private static void Random(int i, int j, bool checkNPCSpawns, int wallDist, bool surface, Action<int, int> perSuccessAction = null)
    {
        List<TileCondition> conditions = TileCondition.GetTileSquare(i, j);

        if (Main.tile[i, j].TileType == TileID.Chlorophyte && !Main.rand.NextBool(8)) //Hardcoded check for nerfing Chlorophyte spread a little
            return;

        if (surface)
            Self.SurfaceRandomUpdate.Invoke(i, j, checkNPCSpawns, wallDist);
        else
            Self.UndergroundRandomUpdate.Invoke(i, j, checkNPCSpawns, wallDist);

        if (conditions.Any(x => x.DiffersFromTile()))
            perSuccessAction?.Invoke(i, j);
    }

    public static void Auto(int i, int j, bool checkNPCSpawns, int wallDist, Action<int, int> perSuccessAction = null) => Random(i, j, checkNPCSpawns, wallDist, j < Main.worldSurface - 1, perSuccessAction);

    public static void CircularUpdate(int x, int y, int radius, int denominator, Action<int, int> perSuccessAction)
    {
        for (int i = x - radius; i < x + radius; ++i)
        {
            for (int j = y - radius; j < y + radius; ++j)
            {
                if (Vector2.DistanceSquared(new Vector2(x, y), new Vector2(i, j)) <= radius * radius && Main.rand.NextBool(denominator))
                {
                    List<TileCondition> conditions = TileCondition.GetTileSquare(i, j);

                    Auto(i, j, false, 3);

                    if (conditions.Any(x => x.DiffersFromTile()))
                        perSuccessAction.Invoke(i, j);
                }
            }
        }
    }

    private struct TileCondition
    {
        internal bool Active;
        internal ushort TileId;
        internal short FrameX;
        internal short FrameY;
        internal Point16 Position;

        public TileCondition(bool active, ushort tileID, short frameX, short frameY, Point16 pos)
        {
            Active = active;
            TileId = tileID;
            FrameX = frameX;
            FrameY = frameY;
            Position = pos;
        }

        public readonly bool DiffersFromTile()
        {
            Tile tile = Main.tile[Position.ToPoint()];

            bool isDifferent = tile.HasTile != Active || tile.TileType != TileId || tile.TileFrameX != FrameX || tile.TileFrameY != FrameY;

            if (isDifferent)
                return true;
            return false;
        }

        public static List<TileCondition> GetTileSquare(int i, int j)
        {
            List<TileCondition> conditions = new();

            for (int x = i - 1; x < i + 2; ++x)
                for (int y = j - 1; y < j + 2; ++y)
                    conditions.Add(TileAt(x, y));

            return conditions;
        }

        public static TileCondition TileAt(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            TileCondition condition = new(tile.HasTile, tile.TileType, tile.TileFrameX, tile.TileFrameY, new Point16(i, j));
            return condition;
        }
    }
}
