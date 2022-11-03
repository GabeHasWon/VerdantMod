using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Verdant.Noise;
using Verdant.Tiles.Verdant.Basic;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Mounted;
using Verdant.Tiles.Verdant.Trees;
using Verdant.Walls;
using Verdant.World.RealtimeGeneration;

namespace Verdant.World
{
    internal class MicroVerdantGen
    {
        public static Queue<RealtimeStep> MicroVerdant()
        {
            Dictionary<Point16, TileAction.TileActionDelegate> orderedActions = new();
            float diameter = VerdantGenSystem.WorldSize * 39;

            GenCircle circle = new((int)diameter, Main.MouseWorld.ToTileCoordinates16());
            circle.FindTiles(false, false);

            VerdantSystem.genNoise = new(Main.rand.Next(int.MaxValue))
            {
                Seed = WorldGen._genRandSeed,
                Frequency = 0.05f,
                NoiseType = FastNoise.NoiseTypes.CubicFractal, //Sets noise to proper type
                FractalType = FastNoise.FractalTypes.Billow
            };

            Point16[] zeniths = new Point16[4] { 
                new Point16(circle.center.X + (int)(diameter - GenCircle.MaxDitherDistance), circle.center.Y),
                new Point16(circle.center.X, circle.center.Y + (int)(diameter - GenCircle.MaxDitherDistance)),
                new Point16(circle.center.X, circle.center.Y - (int)(diameter - GenCircle.MaxDitherDistance) - 0),
                new Point16(circle.center.X - (int)(diameter - GenCircle.MaxDitherDistance), circle.center.Y)
            };

            Point16[] offsets = new Point16[4] { zeniths[0] + new Point16(-1, 0), zeniths[1] + new Point16(0, -1), zeniths[2] + new Point16(0, 1), zeniths[3] + new Point16(1, 0) };

            foreach (var point in circle.tiles)
            {
                float n = VerdantSystem.genNoise.GetNoise(point.X, point.Y);
                TileAction.TileActionDelegate action = (int _, int _, ref bool _) => { };
                float dist = Vector2.DistanceSquared(point.ToVector2(), circle.center.ToVector2());
                double cutoff = Math.Pow(diameter - GenCircle.MaxDitherDistance - 1, 2);

                if (dist > cutoff)
                {
                    if (!zeniths.Contains(point))
                        orderedActions.Add(point, TileAction.PlaceTile(TileID.Glass, false));
                    else
                        orderedActions.Add(offsets[Array.IndexOf(zeniths, point)], TileAction.PlaceTile(TileID.Glass, false));
                    continue;
                }

                if (offsets.Contains(point))
                    continue;

                if (n < -0.67f)
                    action += TileAction.KillTile();
                else if (n < -0.57f)
                    action += TileAction.PlaceTile(ModContent.TileType<VerdantGrassLeaves>(), false);
                else
                    action += TileAction.PlaceTile(ModContent.TileType<LushSoil>(), false);

                if (dist < cutoff)
                {
                    if (n < -0.85f)
                        action += TileAction.PlaceWall(WallID.Glass, force: true);
                    else if (n < -0.52f)
                        action += TileAction.PlaceWall(ModContent.WallType<VerdantLeafWall_Unsafe>(), force: true);
                    else
                        action += TileAction.PlaceWall(ModContent.WallType<LushSoilWall_Unsafe>(), force: true);
                }

                if (action.GetInvocationList().Length > 1)
                    orderedActions.Add(point, action);
            }

            Add1xXFoliage(circle.tiles, orderedActions);

            var queue = new Queue<RealtimeStep>();
            foreach (var key in orderedActions.Keys)
                queue.Enqueue(new(key, orderedActions[key]));

            PostGenFunctions(circle.tiles, queue);

            queue.Enqueue(new(circle.center, (int x, int y, ref bool success) =>
            {
                success = true;
                var size = Point16.Zero;
                StructureHelper.Generator.GetDimensions("World/Structures/ApotheosisGlass", VerdantMod.Instance, ref size);
                Point16 pos = new(x - (size.X / 2), y - (size.Y / 2));
                StructureHelper.Generator.GenerateStructure("World/Structures/ApotheosisGlass", pos, VerdantMod.Instance);

                for (int i = pos.X - 1; i < pos.X + size.X + 1; ++i)
                    for (int j = pos.Y - 1; j < pos.Y + size.Y + 1; ++j)
                        WorldGen.TileFrame(i, j, true, true);
            }));

            foreach (var pos in circle.tiles)
                queue.Enqueue(new(pos, SpawnTree));

            return queue;
        }

        private static void PostGenFunctions(List<Point16> tiles, Queue<RealtimeStep> queue)
        {
            foreach (var pos in tiles)
            {
                queue.Enqueue(new(pos, SpawnVines));
                queue.Enqueue(new(pos, SpawnFlowers));
                queue.Enqueue(new(pos, SpawnWater));
            }

            void Pickipuff(int x, int y, ref bool success)
            {
                int count = 0;
                while (count < 2)
                {
                    var random = Main.rand.Next(tiles);
                    Tile tile = Main.tile[random.ToPoint()];

                    if (tile.HasTile && tile.TileType == ModContent.TileType<VerdantGrassLeaves>())
                    {
                        queue.Enqueue(new(random, (int x, int y, ref bool success) =>
                        {
                            ModContent.GetInstance<Tiles.TileEntities.Puff.Pickipuff>().Place(random.X, random.Y);
                        }));
                        count++;
                    }
                }
            }

            queue.Enqueue(new(Point16.Zero, Pickipuff));
        }

        public static void SpawnWater(int x, int y, ref bool success)
        {
            if (!WorldGen.SolidTile(x, y) && Main.rand.NextBool(12))
            {
                WorldGen.PlaceLiquid(x, y, LiquidID.Water, 255);
                success = true;
            }
        }

        public static void SpawnFlowers(int x, int y, ref bool success)
        {
            int wall = Helper.WalledType(x, y, 2, 2, ModContent.WallType<VerdantLeafWall_Unsafe>()) + Helper.WalledType(x, y, 2, 2, ModContent.WallType<LushSoilWall_Unsafe>());
            if (Helper.AreaClear(x, y, 2, 2) && wall > 3 && Main.rand.NextBool(22))
            {
                WorldGen.PlaceObject(x, y, Main.rand.NextBool(3) ? ModContent.TileType<MountedLightbulb_2x2>() : ModContent.TileType<Flower_2x2>(), true, style: Main.rand.Next(4));
                success = true;
                return;
            }

            wall = Helper.WalledType(x, y, 3, 3, ModContent.WallType<VerdantLeafWall_Unsafe>()) + Helper.WalledType(x, y, 3, 3, ModContent.WallType<LushSoilWall_Unsafe>());
            if (Helper.AreaClear(x, y, 3, 3) && wall > 8 && Main.rand.NextBool(22))
            {
                WorldGen.PlaceObject(x, y, ModContent.TileType<Flower_3x3>(), true, style: Main.rand.Next(2));
                success = true;
            }
        }

        public static void SpawnVines(int x, int y, ref bool success)
        {
            Tile tile = Main.tile[x, y];

            if (WorldGen.TileEmpty(x, y + 1) && tile.HasTile && tile.TileType == ModContent.TileType<VerdantGrassLeaves>() && !tile.BottomSlope && !Main.rand.NextBool(4))
            {
                success = true;
                int length = Main.rand.Next(3, 14);
                bool strong = Main.rand.NextBool(10);

                for (int i = y + 1; i < y + 1 + length; ++i)
                {
                    if (Main.tile[x, i].HasTile)
                        return;

                    WorldGen.PlaceTile(x, i, strong ? ModContent.TileType<VerdantStrongVine>() : ModContent.TileType<VerdantVine>());
                }
            }
        }

        public static void SpawnTree(int x, int y, ref bool success)
        {
            Tile tile = Main.tile[x, y];

            if (Helper.AreaClear(x, y - 6, 1, 5) && tile.HasTile && tile.TileType == ModContent.TileType<VerdantGrassLeaves>() && !tile.TopSlope && Main.rand.NextBool(12))
            {
                VerdantTree.Spawn(x, y - 1, r: Main.rand, leaves: true);
                success = true;
            }
        }

        private static void Add1xXFoliage(List<Point16> tiles, Dictionary<Point16, TileAction.TileActionDelegate> orderedActions)
        {
            foreach (var point in tiles)
            {
                void TrySpawnFoliage(int x, int y, ref bool success)
                {
                    Tile tile = Main.tile[point.ToPoint()];
                    if (Main.rand.NextBool(9) && tile.HasTile && tile.TileType == ModContent.TileType<VerdantGrassLeaves>() && !tile.TopSlope && WorldGen.TileEmpty(x, y - 1))
                    {
                        WeightedRandom<int> size = new();
                        size.Add(1, 1f);
                        size.Add(2, 0.8f);
                        size.Add(3, 0.6f);

                        int chosenSize = size;
                        if (chosenSize == 1)
                            WorldGen.PlaceTile(x, y - 1, Main.rand.NextBool(4) ? ModContent.TileType<VerdantDecor1x1NoCut>() : ModContent.TileType<VerdantDecor1x1>(), style: WorldGen.genRand.Next(7));
                        else if (chosenSize == 2)
                            WorldGen.PlaceTile(x, y - 2, ModContent.TileType<VerdantDecor1x2>(), style: WorldGen.genRand.Next(6));
                        else
                            WorldGen.PlaceTile(x, y - 3, ModContent.TileType<VerdantDecor1x3>(), style: WorldGen.genRand.Next(7));
                        success = true;
                    }
                }

                if (orderedActions.ContainsKey(point))
                    orderedActions[point] += TrySpawnFoliage;
                else
                    orderedActions.Add(point, TrySpawnFoliage);
            }
        }
    }
}
