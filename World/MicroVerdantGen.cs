﻿using Microsoft.Xna.Framework;
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
using Verdant.Systems.RealtimeGeneration;
using Verdant.Tiles.Verdant.Basic.Aquamarine;
using Verdant.Tiles.Verdant.Decor;
using System.Runtime.ExceptionServices;

namespace Verdant.World
{
    internal class MicroVerdantGen
    {
        public static Queue<RealtimeStep> MicroVerdant(Point16 position, bool glassless = false)
        {
            Dictionary<Point16, TileAction.TileActionDelegate> orderedActions = new();
            float diameter = VerdantGenSystem.WorldSize * 39;

            GenCircle circle = new((int)diameter, position);
            circle.FindTiles(false, false);

            RealtimeStep syncStep = new(circle.center, (int x, int y, ref bool success) => //Was gonna use this multiple times but it didn't work oh well
            {
                JustSyncTheWholeThing(x, y, circle.radius);
                success = true;
            });

            VerdantSystem.genNoise = new(WorldGen._genRandSeed)
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

            foreach (var point in GenCircle.Locations)
            {
                float n = VerdantSystem.genNoise.GetNoise(point.X, point.Y);
                TileAction.TileActionDelegate action = (int _, int _, ref bool _) => { };
                float dist = Vector2.DistanceSquared(point.ToVector2(), circle.center.ToVector2());
                double cutoff = Math.Pow(diameter - GenCircle.MaxDitherDistance - 1.5f, 2);

                if (dist > cutoff)
                {
                    if (!zeniths.Contains(point))
                        orderedActions.Add(point, TileAction.PlaceTile(glassless ? ModContent.TileType<VerdantGrassLeaves>() : TileID.Glass, false));
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
                    {
                        if (!glassless)
                            action += TileAction.PlaceWall(WallID.Glass, force: true);
                    }
                    else if (n < -0.52f)
                        action += TileAction.PlaceWall(ModContent.WallType<VerdantLeafWall_Unsafe>(), force: true);
                    else
                        action += TileAction.PlaceWall(ModContent.WallType<LushSoilWall_Unsafe>(), force: true);
                }

                if (action.GetInvocationList().Length > 1)
                    orderedActions.Add(point, action);
            }

            AddOres(GenCircle.Locations, orderedActions);
            Add1xXFoliage(GenCircle.Locations, orderedActions);

            var queue = new Queue<RealtimeStep>();
            foreach (var key in orderedActions.Keys)
                queue.Enqueue(new(key, orderedActions[key]));

            PostGenFunctions(GenCircle.Locations, queue);

            Point16? apothLoc = ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation;

            if (apothLoc.HasValue && (Main.tile[apothLoc.Value].TileType != ModContent.TileType<Apotheosis>() && Main.tile[apothLoc.Value].TileType != ModContent.TileType<HardmodeApotheosis>()))
                Apotheosis.TrySetLocation(apothLoc.Value.X, apothLoc.Value.Y);

            if (!apothLoc.HasValue)
            {
                queue.Enqueue(new(circle.center, (int x, int y, ref bool success) =>
                {
                    success = true;
                    var size = Point16.Zero;
                    StructureHelper.Generator.GetDimensions("World/Structures/ApotheosisGlass", VerdantMod.Instance, ref size);
                    Point16 pos = new(x - (size.X / 2), y - (size.Y / 2));
                    StructureHelper.Generator.GenerateStructure("World/Structures/ApotheosisGlass", pos, VerdantMod.Instance);

                    for (int i = pos.X - 1; i < pos.X + size.X + 1; ++i)
                        for (int j = pos.Y - 1; j < pos.Y + size.Y + 1; ++j)
                            if (Main.hardMode && Main.tile[i, j].TileType == ModContent.TileType<Apotheosis>())
                                Main.tile[i, j].TileType = (ushort)ModContent.TileType<HardmodeApotheosis>();

                    for (int i = pos.X - 1; i < pos.X + size.X + 1; ++i)
                        for (int j = pos.Y - 1; j < pos.Y + size.Y + 1; ++j)
                            WorldGen.TileFrame(i, j);

                    ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation = pos + new Point16(size.X / 2, size.Y / 2);
                }));
            }

            foreach (var pos in GenCircle.Locations)
                queue.Enqueue(new(pos, SpawnTree));

            if (Main.netMode == NetmodeID.Server)
                queue.Enqueue(syncStep);

            GenCircle.Locations.Clear();
            return queue;
        }

        private static void JustSyncTheWholeThing(int x, int y, int radius)
        {
            int halfRadius = radius / 2;
            if (radius < 90)
            {
                NetMessage.SendTileSquare(-1, x - halfRadius, y - halfRadius, radius);
                NetMessage.SendTileSquare(-1, x + halfRadius, y - halfRadius, radius);
                NetMessage.SendTileSquare(-1, x - halfRadius, y + halfRadius, radius);
                NetMessage.SendTileSquare(-1, x + halfRadius, y + halfRadius, radius);
            }
            else
            {
                int fourthRadius = radius / 4;

                for (int i = -1; i < 6; ++i)
                    for (int j = -1; j < 6; ++j)
                        NetMessage.SendTileSquare(-1, x - halfRadius + (i * fourthRadius), y - halfRadius + (j * fourthRadius), halfRadius);
            }
        }

        private static void AddOres(HashSet<Point16> tiles, Dictionary<Point16, TileAction.TileActionDelegate> orderedActions)
        {
            foreach (var point in tiles)
            {
                static void AddVeins(int x, int y, ref bool success)
                {
                    if (WorldGen.genRand.NextBool(120))
                    {
                        WorldGen.TileRunner(x, y, 5, 4, ModContent.TileType<BackslateTile>(), false, 0, 0, false, true);
                        success = true;
                    }

                    if (WorldGen.genRand.NextBool(80))
                    {
                        WorldGen.TileRunner(x, y, 2, 8, ModContent.TileType<EmbeddedAquamarine>(), false, 0, 0, false, true);
                        success = true;
                    }
                }

                if (orderedActions.ContainsKey(point))
                    orderedActions[point] += AddVeins;
                else
                    orderedActions.Add(point, AddVeins);
            }
        }

        private static void PostGenFunctions(HashSet<Point16> tiles, Queue<RealtimeStep> queue)
        {
            foreach (var pos in tiles)
            {
                queue.Enqueue(new(pos, SpawnVines));
                queue.Enqueue(new(pos, SpawnFlowers));

                if (Main.netMode == NetmodeID.SinglePlayer)
                    queue.Enqueue(new(pos, SpawnWater));
            }

            void Pickipuff(int x, int y, ref bool success)
            {
                int count = 0;

                while (count < 2)
                {
                    if (!tiles.Any())
                        break;

                    var random = WorldGen.genRand.Next(tiles.ToList());
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
            if (!WorldGen.SolidTile(x, y) && WorldGen.genRand.NextBool(10))
            {
                WorldGen.PlaceLiquid(x, y, (byte)LiquidID.Water, 255);
                success = true;
            }
        }

        public static void SpawnFlowers(int x, int y, ref bool success)
        {
            int wall = Helper.WalledType(x, y, 2, 2, ModContent.WallType<VerdantLeafWall_Unsafe>()) + Helper.WalledType(x, y, 2, 2, ModContent.WallType<LushSoilWall_Unsafe>());
            if (Helper.AreaClear(x, y, 2, 2) && wall > 3 && WorldGen.genRand.NextBool(22))
            {
                WorldGen.PlaceObject(x, y, WorldGen.genRand.NextBool(3) ? ModContent.TileType<MountedLightbulb_2x2>() : ModContent.TileType<Flower_2x2>(), true, style: WorldGen.genRand.Next(4));
                success = true;
                return;
            }

            wall = Helper.WalledType(x, y, 3, 3, ModContent.WallType<VerdantLeafWall_Unsafe>()) + Helper.WalledType(x, y, 3, 3, ModContent.WallType<LushSoilWall_Unsafe>());
            if (Helper.AreaClear(x, y, 3, 3) && wall > 8 && WorldGen.genRand.NextBool(22))
            {
                WorldGen.PlaceObject(x, y, ModContent.TileType<Flower_3x3>(), true, style: WorldGen.genRand.Next(2));
                success = true;
            }
        }

        public static void SpawnVines(int x, int y, ref bool success)
        {
            Tile tile = Main.tile[x, y];

            if (WorldGen.TileEmpty(x, y + 1) && tile.HasTile && tile.TileType == ModContent.TileType<VerdantGrassLeaves>() && !tile.BottomSlope && !WorldGen.genRand.NextBool(4))
            {
                success = true;
                int length = WorldGen.genRand.Next(3, 14);
                bool strong = WorldGen.genRand.NextBool(10);

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

            if (Helper.AreaClear(x, y - 6, 1, 5) && tile.HasTile && tile.TileType == ModContent.TileType<VerdantGrassLeaves>() && !tile.TopSlope && WorldGen.genRand.NextBool(12))
            {
                VerdantTree.Spawn(x, y - 1, r: WorldGen.genRand, leaves: true);
                success = true;
            }
        }

        private static void Add1xXFoliage(HashSet<Point16> tiles, Dictionary<Point16, TileAction.TileActionDelegate> orderedActions)
        {
            foreach (var point in tiles)
            {
                void TrySpawnFoliage(int x, int y, ref bool success)
                {
                    Tile tile = Main.tile[point.ToPoint()];
                    if (WorldGen.genRand.NextBool(9) && tile.HasTile && tile.TileType == ModContent.TileType<VerdantGrassLeaves>() && !tile.TopSlope && WorldGen.TileEmpty(x, y - 1))
                    {
                        WeightedRandom<int> size = new();
                        size.Add(1, 1f);
                        size.Add(2, 0.8f);
                        size.Add(3, 0.6f);

                        int chosenSize = size;
                        if (chosenSize == 1)
                            WorldGen.PlaceTile(x, y - 1, WorldGen.genRand.NextBool(4) ? ModContent.TileType<VerdantDecor1x1NoCut>() : ModContent.TileType<VerdantDecor1x1>(), style: WorldGen.genRand.Next(7));
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
