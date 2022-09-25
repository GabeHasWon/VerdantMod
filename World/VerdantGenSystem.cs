using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.WorldBuilding;
using Verdant.Noise;
using Verdant.Walls;
using Verdant.Tiles.Verdant.Decor;
using Verdant.Items.Verdant.Tools;
using Verdant.Tiles.Verdant.Basic;
using Verdant.Tiles.Verdant.Trees;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Tiles.Verdant.Mounted;
using Verdant.Items.Verdant.Weapons;
using Verdant.Items.Verdant.Materials;
using Verdant.Items.Verdant.Equipables;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles;
using Terraria.IO;
using System;

namespace Verdant.World
{
    ///Handles specific Verdant biome gen.
    public class VerdantGenSystem : ModSystem
    {
        public static float WorldSize { get => Main.maxTilesX / 4200f; }

        private static int[] TileTypes { get => new int[] { ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>(), TileID.ChlorophyteBrick, ModContent.TileType<VerdantLightbulb>(), ModContent.TileType<LivingLushWood>() }; }
        private static int[] WallTypes { get => new int[] { ModContent.WallType<VerdantLeafWall_Unsafe>(), ModContent.WallType<LushSoilWall_Unsafe>(), ModContent.WallType<LivingLushWoodWall_Unsafe>() }; }

        public static Point VerdantCentre = new Point();
        public static Rectangle VerdantArea = new Rectangle(0, 0, 0, 0);

        private readonly List<GenCircle> VerdantCircles = new List<GenCircle>();

        public void VerdantGeneration(GenerationProgress p, GameConfiguration config)
        {
            p.Message = "Growing plants...";

            Mod.Logger.Info("World Seed: " + WorldGen._genRandSeed);
            Mod.Logger.Info("Noise Seed: " + VerdantSystem.genNoise.Seed);

            VerdantCentre = new Point(WorldGen.genRand.Next(Main.maxTilesX / 3, (int)(Main.maxTilesX / 1.5f)), WorldGen.genRand.Next((int)(Main.maxTilesY / 2.1f), (int)(Main.maxTilesY / 1.75f)));

            int FluffX = (int)(220 * WorldSize);
            int FluffY = (int)(130 * WorldSize);

            int total = 0;
            while (true) //Find valid position for biome
            {
            reset:
                VerdantCentre = new Point(WorldGen.genRand.Next(Main.maxTilesX / 4, (int)(Main.maxTilesX / 1.20f)), WorldGen.genRand.Next((int)(Main.maxTilesY / 2.5f), (int)(Main.maxTilesY / 1.75f)));
                total = 0;
                if (WorldGen.UndergroundDesertLocation.Contains(VerdantCentre.X - FluffX, VerdantCentre.Y - FluffY) || WorldGen.UndergroundDesertLocation.Contains(VerdantCentre.X - FluffX, VerdantCentre.Y + FluffY)
                    || WorldGen.UndergroundDesertLocation.Contains(VerdantCentre.X + FluffX, VerdantCentre.Y - FluffY) || WorldGen.UndergroundDesertLocation.Contains(VerdantCentre.X + FluffX, VerdantCentre.Y + FluffY)
                    || WorldGen.UndergroundDesertLocation.Contains(VerdantCentre))
                    continue;
                for (int i = VerdantCentre.X - (int)(FluffX * 1.2f); i < VerdantCentre.X + (FluffX * 1.2f); ++i) //Assume width
                {
                    for (int j = VerdantCentre.Y - 140; j < VerdantCentre.Y + 140; ++j) //Assume height
                    {
                        List<int> invalidTypes = new() { TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick, TileID.LihzahrdBrick, TileID.IceBlock, TileID.SnowBlock }; //Vanilla blacklist

                        if (ModLoader.TryGetMod("SpiritMod", out Mod spirit)) //Spirit blacklist
                            invalidTypes.Add(spirit.Find<ModTile>("BriarGrass").Type);
                        if (ModLoader.TryGetMod("CalamityMod", out Mod calamity)) //Calamity blacklist
                            invalidTypes.Add(calamity.Find<ModTile>("Navystone").Type);

                        if ((Framing.GetTileSafely(i, j).HasTile && invalidTypes.Any(x => Framing.GetTileSafely(i, j).TileType == x)))
                            total++;
                        if (total > 50)
                            goto reset;
                    }
                }
                break;
            }

            GenerateCircles();
            CleanForCaves();
            AddStone();

            for (int i = VerdantArea.Left; i < VerdantArea.Right; ++i) //Smooth out the biome!
                for (int j = VerdantArea.Top; j < VerdantArea.Bottom; ++j)
                    if (WorldGen.genRand.Next(7) <= 3)
                        Tile.SmoothSlope(i, j, false);

            p.Message = "Growing vines...";
            Vines();
            p.Message = "Growing flowers...";
            AddPlants();
            p.Message = "Watering plants...";
            AddWater();
            AddWaterfalls();
            p.Message = "Growing surface...";
            AddSurfaceVerdant();
        }

        private static void AddWaterfalls()
        {
            for (int i = 0; i < 50 * WorldSize; ++i)
            {
                int x = Main.rand.Next(VerdantArea.Left, VerdantArea.Right);
                int y = Main.rand.Next(VerdantArea.Top, VerdantArea.Bottom);
                Tile tile = Main.tile[x, y];

                if (!WorldGen.SolidTile(tile))
                    DigFall(x, y, 2);
                else
                    --i;
            }
        }

        private static void DigFall(int x, int y, int minHeight)
        {
            bool CanPlaceAt(int i, int j, int dir) => WorldGen.SolidTile(i, j) && WorldGen.SolidTile(i + dir, j) && WorldGen.SolidTile(i + dir + dir, j - 1);

            void PlaceAt(int i, int j, int dir)
            {
                Tile liquidTile = Main.tile[i + dir, j - 1];
                liquidTile.ClearTile();
                liquidTile.LiquidAmount = 255;
                liquidTile.LiquidType = LiquidID.Water;

                Tile halfTile = Main.tile[i, j - 1];
                halfTile.IsHalfBlock = true;

                if (halfTile.TileType == TileID.Silt || liquidTile.TileType == TileID.Sand)
                    liquidTile.TileType = TileID.Stone;

                Tile silt = Main.tile[i + dir + dir, j - 1];
                if (silt.TileType == TileID.Silt || silt.TileType == TileID.Sand)
                    silt.TileType = TileID.Stone;

                silt = Main.tile[i + dir, j];
                if (silt.TileType == TileID.Silt || silt.TileType == TileID.Sand)
                    silt.TileType = TileID.Stone;

                int adjY = 0;
                while (Main.tile[i - dir, j - 1 + adjY].HasTile)
                    Main.tile[i - dir, j - 1 + adjY++].ClearTile();
            }

            const int MaxHeight = 16;

            for (int j = y; j > y - MaxHeight; --j)
            {
                if (Math.Abs(j - y) < minHeight)
                    continue;

                if (CanPlaceAt(x, j, -1))
                {
                    PlaceAt(x, j, -1);
                    return;
                }
                else if (CanPlaceAt(x, j, 1))
                {
                    PlaceAt(x, j, 1);
                    return;
                }
            }
        }

        private static void AddSurfaceVerdant()
        {
            int offset = 0;
        retry:
            int top = Helper.FindDown(new Vector2((VerdantArea.Center.X + (WorldGen.genRand.NextBool() ? -offset : offset)) * 16, 200));
            Point16 size = Point16.Zero;

            if (top <= Main.worldSurface * 0.36f)
            {
                offset += offset + WorldGen.genRand.Next(10, 20);
                goto retry;
            }

            if (StructureHelper.Generator.GetDimensions("World/Structures/SurfaceTree", VerdantMod.Instance, ref size)) 
                StructureHelper.Generator.GenerateStructure("World/Structures/SurfaceTree", new Point16(VerdantArea.Center.X - (size.X / 2), top - size.Y + 12), VerdantMod.Instance);
        }

        public void VerdantCleanup(GenerationProgress p, GameConfiguration config)
        {
            p.Message = "Trimming plants...";

            AddFlowerStructures();
            PlaceApotheosis();

            for (int i = VerdantArea.Right; i > VerdantArea.X; --i)
            {
                for (int j = VerdantArea.Bottom; j > VerdantArea.Y; --j)
                {
                    Tile tile = Main.tile[i, j];
                    tile.LiquidType = LiquidID.Water;

                    Tile t = Framing.GetTileSafely(i, j);
                    int[] vineAnchors = new int[] { ModContent.TileType<VerdantVine>(), ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<VerdantLeaves>() };
                    if (t.TileType == ModContent.TileType<VerdantVine>() && !vineAnchors.Contains(Framing.GetTileSafely(i, j - 1).TileType))
                        WorldGen.KillTile(i, j);
                }
            }
        }

        private void PlaceApotheosis()
        {
            int[] invalidTypes = new int[] { TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick, TileID.LihzahrdBrick };
            Point apothPos = new Point(VerdantArea.Center.X - 10, VerdantArea.Center.Y - 4);
            int side = WorldGen.genRand.NextBool(2) ? -1 : 1;
            bool triedOneSide = false;

        redo:
            for (int i = 0; i < 20; ++i)
            {
                for (int j = 0; j < 18; ++j)
                {
                    Tile t = Framing.GetTileSafely(apothPos.X + i, apothPos.Y + j);
                    if (t.HasTile && invalidTypes.Contains(t.TileType))
                    {
                        apothPos.X += WorldGen.genRand.Next(20, 27) * side;
                        if (!VerdantArea.Contains(apothPos) && !triedOneSide)
                        {
                            triedOneSide = true;
                            apothPos = new Point(VerdantArea.Center.X - 30, VerdantArea.Center.Y - 4);
                            side *= -1;
                        }
                        goto redo; //sorry but i had to
                    }
                }
            }
            StructureHelper.Generator.GenerateStructure("World/Structures/Apotheosis", new Point16(apothPos.X, apothPos.Y), VerdantMod.Instance);
        }

        private void AddFlowerStructures()
        {
            Point[] offsets = new Point[3] { new Point(7, -1), new Point(3, 0), new Point(3, 0) }; //ruler in-game is ONE HIGHER on both planes
            int[] invalids = new int[] { TileID.LihzahrdBrick, TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick, ModContent.TileType<Apotheosis>() };
            int[] valids = new int[] { ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>() };

            List<Vector2> positions = new List<Vector2>() { new Vector2(VerdantArea.Center.X - 10, VerdantArea.Center.Y - 4) }; //So I don't overlap with the Apotheosis

            for (int i = 0; i < 9 * WorldSize; ++i)
            {
                int index = Main.rand.Next(offsets.Length);
                Point16 pos = new Point16(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));

                bool notNear = !positions.Any(x => Vector2.Distance(x, pos.ToVector2()) < 20);

                if (notNear && Helper.TileRectangle(pos.X, pos.Y, 20, 10, valids) > 4 && Helper.TileRectangle(pos.X, pos.Y, 20, 10, invalids) <= 0 && Helper.NoTileRectangle(pos.X, pos.Y, 20, 10) > 40)
                {
                    StructureHelper.Generator.GenerateMultistructureSpecific("World/Structures/Flowers", pos, Mod, index);
                    positions.Add(pos.ToVector2());

                    int x = pos.X + offsets[index].X;
                    int y = pos.Y + offsets[index].Y + 2;

                    if (!WorldGen.genRand.NextBool(4)) //NORMAL chests
                    {
                        bool c = GenHelper.PlaceChest(x, y, ModContent.TileType<VerdantYellowPetalChest>(), new (int, int)[]
                        {
                            (ModContent.ItemType<VerdantStaff>(), 1), (ModContent.ItemType<Lightbloom>(), 1), (ModContent.ItemType<ExpertPlantGuide>(), 1), (ModContent.ItemType<Halfsprout>(), WorldGen.genRand.Next(20, 31))
                        }, new (int, int)[] {
                            (ItemID.IronskinPotion, WorldGen.genRand.Next(1, 3)), (ItemID.ThornsPotion, WorldGen.genRand.Next(1, 3)), (ItemID.ThrowingKnife, WorldGen.genRand.Next(3, 7)),
                            (ModContent.ItemType<PinkPetal>(), WorldGen.genRand.Next(3, 7)), (ModContent.ItemType<RedPetal>(), WorldGen.genRand.Next(3, 7)), (ModContent.ItemType<Lightbulb>(), WorldGen.genRand.Next(1, 3)),
                            (ItemID.Dynamite, 1), (ItemID.Glowstick, WorldGen.genRand.Next(3, 8)), (ItemID.Glowstick, WorldGen.genRand.Next(3, 8)), (ItemID.Bomb, WorldGen.genRand.Next(2, 4)),
                            (ItemID.NightOwlPotion, WorldGen.genRand.Next(2, 4)), (ItemID.HealingPotion, WorldGen.genRand.Next(2, 4)), (ItemID.MoonglowSeeds, WorldGen.genRand.Next(2, 4)),
                            (ItemID.DaybloomSeeds, WorldGen.genRand.Next(2, 4)), (ItemID.BlinkrootSeeds, WorldGen.genRand.Next(2, 4))
                        }, false, WorldGen.genRand, WorldGen.genRand.Next(6, 9), 0, false);

                        if (!c)
                            Mod.Logger.Warn("Failed to place Verdant Yellow Petal Chest.");
                    }
                    else //WAND chest
                    {
                        bool c = GenHelper.PlaceChest(x, y, ModContent.TileType<VerdantYellowPetalChest>(), 0, false,
                            Helper.ItemStack<LushLeafWand>(), Helper.ItemStack<PinkPetalWand>(), Helper.ItemStack<RedPetalWand>(), Helper.ItemStack<RedPetal>(WorldGen.genRand.Next(19, 24)),
                            (ModContent.ItemType<PinkPetal>(), WorldGen.genRand.Next(19, 24)), (ModContent.ItemType<VerdantFlowerBulb>(), WorldGen.genRand.Next(12, 22)));

                        if (!c)
                            Mod.Logger.Warn("Failed to place Verdant Yellow Petal Chest (wand).");
                    }
                }
                else
                {
                    i--;
                    continue;
                }
            }
        }

        private void AddStone()
        {
            for (int i = 0; i < 60 * WorldSize; ++i) //Stones
            {
                Point p = new(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                while (!TileHelper.ActiveType(p.X, p.Y, ModContent.TileType<LushSoil>()))
                    p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                WorldGen.TileRunner(p.X, p.Y, WorldGen.genRand.NextFloat(7, 15), WorldGen.genRand.Next(5, 15), TileID.Stone, false, 0, 0, false, true);
            }

            for (int i = 0; i < 10 * WorldSize; ++i) //Ores
            {
                Point p = new(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                while (!TileHelper.ActiveType(p.X, p.Y, ModContent.TileType<LushSoil>()))
                    p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                WorldGen.TileRunner(p.X, p.Y, WorldGen.genRand.NextFloat(2, 8), WorldGen.genRand.Next(5, 15), TileID.Gold, false, 0, 0, false, true);
                p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                while (!TileHelper.ActiveType(p.X, p.Y, ModContent.TileType<LushSoil>()))
                    p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                WorldGen.TileRunner(p.X, p.Y, WorldGen.genRand.NextFloat(2, 7), WorldGen.genRand.Next(5, 15), TileID.Platinum, false, 0, 0, false, true);
            }
        }

        private static void AddWater()
        {
            for (int i = 0; i < 26 * WorldSize; ++i)
            {
                Point p = new(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                for (int j = -14; j < 14; ++j)
                {
                    for (int k = -14; k < 14; ++k)
                    {
                        Tile tile = Main.tile[p.X + j, p.Y + k];
                        tile.LiquidAmount = 255;
                        tile.LiquidType = 0;
                    }
                }
            }
        }

        private static void Vines()
        {
            for (int i = 0; i < 220 * WorldSize; ++i)
            {
                Point rP = new(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                Point adj = TileHelper.GetRandomOpenAdjacent(rP.X, rP.Y);
                while (adj == new Point(-2, -2) || adj == new Point(0, -1) || adj == new Point(0, 1))
                {
                    rP = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                    adj = TileHelper.GetRandomOpenAdjacent(rP.X, rP.Y);
                }
                Tile tile = Framing.GetTileSafely(rP.X, rP.Y);

                if (tile.TileType == ModContent.TileType<VerdantGrassLeaves>() || tile.TileType == ModContent.TileType<LushSoil>() || tile.TileType == ModContent.TileType<LivingLushWood>())
                {
                    Point adjPos = rP.Add(adj);
                    Point end = adjPos.Add(adj);

                    while (true)
                    {
                        end = end.Add(adj);
                        if (TileHelper.SolidTile(end.X - adj.X, end.Y - adj.Y))
                            break;
                    }
                    
                    int midPointY = ((adjPos.Y + end.Y) / 2) + WorldGen.genRand.Next(10, 20);
                    int thickness = WorldGen.genRand.Next(1, 4);

                    if (WorldGen.genRand.Next(3) > 0)
                    {
                        for (int k = 0; k < thickness; ++k)
                            GenHelper.GenBezierDirectWall(new double[] {
                                adjPos.X, adjPos.Y - k,
                                ((adjPos.X + end.X) / 2), midPointY - k,
                                end.X, end.Y - k,
                            }, 200, ModContent.WallType<VerdantVineWall_Unsafe>(), true, 1);
                    }
                    else
                    {
                        for (int k = 0; k < thickness; ++k)
                            GenHelper.GenBezierDirect(new double[] {
                                adjPos.X, adjPos.Y - k,
                                ((adjPos.X + end.X) / 2), midPointY - k,
                                end.X, end.Y - k,
                            }, 200, ModContent.TileType<VerdantLeaves>(), false, 1);
                    }
                }
                else if (WorldGen.genRand.Next(4) > 0)
                    i--;
            }
        }

        private static void AddPlants()
        {
            for (int i = VerdantArea.X; i < VerdantArea.Right; ++i)
            {
                for (int j = VerdantArea.Y; j < VerdantArea.Bottom; ++j) //Loop explicitly for trees so they get all the spawns they need
                {
                    bool doPlace = true;

                    for (int k = -1; k < 2; ++k)
                    {
                        bool anyConditions = !TileHelper.ActiveTypeNoTopSlope(i + k, j, TileTypes[0]) || !WorldGen.TileEmpty(i + k, j - 1);
                        if (anyConditions)
                        {
                            doPlace = false;
                            break;
                        }
                    }

                    if (!WorldGen.TileEmpty(i, j - 2))
                        doPlace = false;

                    if (doPlace && WorldGen.genRand.NextBool(30))
                        VerdantTree.Spawn(i, j - 1, -1, WorldGen.genRand, 4, 12, false, -1, false);
                }
            }

            for (int i = VerdantArea.X; i < VerdantArea.Right; ++i)
            {
                for (int j = VerdantArea.Y; j < VerdantArea.Bottom; ++j)
                {
                    if (TileHelper.ActiveType(i, j, ModContent.TileType<VerdantGrassLeaves>()))
                    {
                        //Vines
                        if (!Framing.GetTileSafely(i, j + 1).HasTile && !Framing.GetTileSafely(i, j + 1).BottomSlope && WorldGen.genRand.Next(5) <= 2)
                        {
                            int length = WorldGen.genRand.Next(2, 22);
                            bool strong = WorldGen.genRand.NextBool(10);

                            for (int l = 1; l < length; ++l)
                            {
                                if (Framing.GetTileSafely(i, j + l + 1).HasTile)
                                    break;

                                WorldGen.KillTile(i, j + l, false, false, true); //please
                                WorldGen.PlaceTile(i, j + l, strong ? ModContent.TileType<VerdantStrongVine>() : ModContent.TileType<VerdantVine>(), true, true);
                                Framing.GetTileSafely(i, j + l).TileType = (ushort)(strong ? ModContent.TileType<VerdantStrongVine>() : ModContent.TileType<VerdantVine>());

                                if (strong)
                                    Framing.GetTileSafely(i, j + l).TileFrameY = (short)(Main.rand.Next(4) * 18);
                            }
                            continue;
                        }
                    }

                    //lightbulb
                    bool doPlace = Helper.AreaClear(i, j - 2, 2, 2) && TileHelper.ActiveTypeNoTopSlope(i, j, ModContent.TileType<VerdantGrassLeaves>()) && TileHelper.ActiveTypeNoTopSlope(i + 1, j, ModContent.TileType<VerdantGrassLeaves>());
                    if (doPlace && WorldGen.genRand.NextBool(11))
                    {
                        WorldGen.PlaceTile(i, j - 2, ModContent.TileType<VerdantLightbulb>(), true, false, -1, WorldGen.genRand.Next(3));
                        continue;
                    }

                    //beehive
                    doPlace = Helper.AreaClear(i, j - 2, 2, 2) && TileHelper.ActiveTypeNoTopSlope(i, j, ModContent.TileType<VerdantGrassLeaves>()) && TileHelper.ActiveTypeNoTopSlope(i + 1, j, ModContent.TileType<VerdantGrassLeaves>());
                    if (doPlace && WorldGen.genRand.NextBool(40))
                    {
                        WorldGen.PlaceTile(i, j - 2, ModContent.TileType<Beehive>(), true, false);
                        continue;
                    }

                    if (!Framing.GetTileSafely(i, j - 1).HasTile && !TileHelper.ActiveTypeNoTopSlope(i, j, ModContent.TileType<VerdantGrassLeaves>()) && WorldGen.genRand.Next(5) >= 1)
                    {
                        int type = !Main.rand.NextBool(1) ? ModContent.TileType<VerdantDecor1x1>() : ModContent.TileType<VerdantDecor1x1NoCut>();
                        WorldGen.PlaceTile(i, j - 1, type, true, false, -1, WorldGen.genRand.Next(7));
                        continue;
                    }

                    //ground decor 2x1
                    doPlace = !Framing.GetTileSafely(i, j - 1).HasTile && TileHelper.ActiveTypeNoTopSlope(i, j, ModContent.TileType<VerdantGrassLeaves>()) &&
                        !Framing.GetTileSafely(i + 1, j - 1).HasTile && TileHelper.ActiveTypeNoTopSlope(i + 1, j, ModContent.TileType<VerdantGrassLeaves>());
                    if (doPlace && WorldGen.genRand.NextBool(2))
                    {
                        GenHelper.PlaceMultitile(new Point(i, j - 1), ModContent.TileType<VerdantDecor2x1>(), WorldGen.genRand.Next(7));
                        continue;
                    }

                    //flower wall 2x2
                    doPlace = Helper.AreaClear(i, j, 2, 2) && Helper.WalledSquare(i, j, 2, 2) && Helper.WalledSquareType(i, j, 2, 2, WallTypes[0]);
                    if (doPlace && WorldGen.genRand.NextBool(42))
                    {
                        int type = WorldGen.genRand.NextBool(13) ? ModContent.TileType<MountedLightbulb_2x2>() : ModContent.TileType<Flower_2x2>();
                        GenHelper.PlaceMultitile(new Point(i, j), type, WorldGen.genRand.Next(type == ModContent.TileType<MountedLightbulb_2x2>() ? 2 : 4));
                        continue;
                    }

                    //flower wall 3x3
                    doPlace = Helper.AreaClear(i, j, 3, 3) && Helper.WalledSquare(i, j, 3, 3) && Helper.WalledSquareType(i, j, 3, 3, WallTypes[0]);
                    if (doPlace && WorldGen.genRand.NextBool(68))
                    {
                        GenHelper.PlaceMultitile(new Point(i, j), ModContent.TileType<Flower_3x3>(), WorldGen.genRand.Next(2));
                        continue;
                    }
                }
            }
        }

        private void GenerateCircles()
        {
            float repeats = 8 * WorldSize;

            VerdantArea = new Rectangle(VerdantCentre.X - (int)(3 * WorldSize * WorldGen.genRand.Next(75, 85)) - 20, VerdantCentre.Y - 100, (int)(8 * WorldSize * WorldGen.genRand.Next(75, 85)), 200);
            VerdantArea.Location = new Point(VerdantArea.Location.X - 40, VerdantArea.Location.Y - 40);
            VerdantArea.Width += 80;
            VerdantArea.Height += 80;

            VerdantCircles.Clear();
            for (int i = 0; i < repeats; ++i)
            {
                int x = (int)MathHelper.Lerp(VerdantArea.X + 50, VerdantArea.Right - 50,  i / repeats);
                int y = VerdantArea.Center.Y - WorldGen.genRand.Next(-20, 20);

                VerdantCircles.Add(new GenCircle((int)(WorldGen.genRand.Next(50, 80) * WorldSize), new Point(x, y)));
            }

            for (int i = 0; i < VerdantCircles.Count; ++i)
                VerdantCircles[i].Gen();
        }

        private void CleanForCaves()
        {
            const float Buffer = 3f;

            TunnelSpice();

            //Caves
            VerdantSystem.genNoise.Seed = WorldGen._genRandSeed;
            VerdantSystem.genNoise.Frequency = 0.05f;
            VerdantSystem.genNoise.NoiseType = FastNoise.NoiseTypes.CubicFractal; //Sets noise to proper type
            VerdantSystem.genNoise.FractalType = FastNoise.FractalTypes.Billow;

            for (int i = VerdantCentre.X - (int)(Main.maxTilesX / Buffer); i < VerdantCentre.X + (int)(Main.maxTilesX / Buffer); ++i)
            {
                if (i < 2) 
                    i = 2;
                if (i > Main.maxTilesX - 2) 
                    break;

                for (int j = VerdantCentre.Y - (int)(Main.maxTilesY / (Buffer * 3)); j < VerdantCentre.Y + (int)(Main.maxTilesY / (Buffer * 3)); ++j)
                {
                    if (j < 2) 
                        j = 2;
                    if (j > Main.maxTilesY - 2) 
                        break;

                    Tile t = Framing.GetTileSafely(i, j);
                    if (t.HasTile && t.TileType == TileTypes[2])
                    {
                        float n = VerdantSystem.genNoise.GetNoise(i, j);
                        t.ClearTile();
                        if (n < -0.67f) { }
                        else if (n < -0.57f) 
                            WorldGen.PlaceTile(i, j, TileTypes[0]);
                        else 
                            WorldGen.PlaceTile(i, j, TileTypes[1]);

                        if (n < -0.85f) 
                            WorldGen.KillWall(i, j, false);
                        else if (n < -0.52f) 
                            WorldGen.PlaceWall(i, j, WallTypes[0]);
                        else 
                            WorldGen.PlaceWall(i, j, WallTypes[1]);
                    }
                }
            }

            //Roots
            VerdantSystem.genNoise.Seed = WorldGen._genRandSeed;
            VerdantSystem.genNoise.Frequency = 0.014f;
            VerdantSystem.genNoise.NoiseType = FastNoise.NoiseTypes.ValueFractal;
            VerdantSystem.genNoise.FractalType = FastNoise.FractalTypes.Billow;
            VerdantSystem.genNoise.InterpolationMethod = FastNoise.Interp.Quintic;

            for (int i = VerdantCentre.X - (int)(Main.maxTilesX / Buffer); i < VerdantCentre.X + (int)(Main.maxTilesX / Buffer); ++i)
            {
                for (int j = VerdantCentre.Y - (int)(Main.maxTilesY / (Buffer * 2)); j < VerdantCentre.Y + (int)(Main.maxTilesY / (Buffer * 2)); ++j)
                {
                    Tile t = Framing.GetTileSafely(i, j);
                    float n = VerdantSystem.genNoise.GetNoise(i, j);
                    if (t.WallType == WallTypes[0] && n < -0.4f)
                        GenHelper.ReplaceWall(new Point(i, j), WallTypes[2]);

                    if (n < -0.72f && TileTypes.Any(x => x == t.TileType) && t.TileType != TileTypes[0] && t.HasTile)
                        GenHelper.ReplaceTile(new Point(i, j), TileTypes[4]);
                }
            }
        }

        private void TunnelSpice()
        {
            int runners = 0;
            while (runners < 20)
            {
                Point position = new Point(VerdantArea.X + WorldGen.genRand.Next(VerdantArea.Width), VerdantArea.Y + WorldGen.genRand.Next(VerdantArea.Height));

                bool ver = false;
                bool non = false;
                for (int j = -8; j < 8; ++j)
                {
                    if (ver && non)
                        break;
                    for (int k = -8; k < 8; ++k)
                    {
                        if (Framing.GetTileSafely(position.X + j, position.Y + k).HasTile)
                        {
                            if (TileTypes.Contains(Framing.GetTileSafely(position.X + j, position.Y + k).TileType))
                                ver = true;
                            else
                                non = true;
                        }
                    }
                }

                if (!ver || !non)
                {
                    if (WorldGen.genRand.NextBool(9)) runners++;
                    continue;
                }

                Vector2 speed = new Vector2(0, WorldGen.genRand.NextFloat(8, 12)).RotatedByRandom(MathHelper.Pi);
                WorldGen.TileRunner(position.X, position.Y, WorldGen.genRand.Next(45, 57) * WorldSize, (int)(WorldGen.genRand.Next(50, 89) * WorldSize), TileTypes[2], true, speed.X, speed.Y, false, true);
                runners++;
            }
        }

        public override void PostWorldGen() //Final cleanup
        {
            for (int i = VerdantArea.Right; i > VerdantArea.X; --i)
            {
                for (int j = VerdantArea.Bottom; j > VerdantArea.Y; --j)
                {
                    if (TileHelper.ActiveType(i, j, ModContent.TileType<VerdantLillie>()) && Framing.GetTileSafely(i, j).LiquidAmount < 155)
                        WorldGen.KillTile(i, j, false, false, true);

                    if (TileHelper.ActiveType(i, j, ModContent.TileType<VerdantTree>()) && !TileHelper.ActiveType(i, j + 1, ModContent.TileType<VerdantTree>()) && !TileHelper.ActiveType(i, j + 1, ModContent.TileType<VerdantGrassLeaves>()))
                        WorldGen.KillTile(i, j, false, false, true);
                }
            }

            for (int i = VerdantArea.X; i < VerdantArea.Right; ++i)
            {
                for (int j = VerdantArea.Y; j < VerdantArea.Bottom; ++j)
                {
                    if (TileHelper.ActiveType(i, j, ModContent.TileType<VerdantStrongVine>()) && !TileHelper.ActiveType(i, j - 1, ModContent.TileType<VerdantStrongVine>()) && !TileHelper.ActiveType(i, j - 1, ModContent.TileType<VerdantGrassLeaves>()))
                        WorldGen.KillTile(i, j, false, false, true);
                }
            }
        }

        /// <summary>Simple struct for genning the base shape of the Verdant.</summary>
        internal struct GenCircle
        {
            public int rad;
            public Point pos;

            public GenCircle(int r, Point p)
            {
                rad = r;
                pos = p;
            }

            public override string ToString() => rad + " + " + pos;

            public void Gen()
            {
                const float MaxDitherDistance = 8f;

                for (int i = -rad; i < rad; ++i)
                {
                    for (int j = -rad; j < rad; ++j)
                    {
                        Point nPos = new Point(pos.X + i, pos.Y + j);
                        float dist = Vector2.Distance(nPos.ToVector2(), pos.ToVector2());
                        Tile tile = Framing.GetTileSafely(nPos.X, nPos.Y);

                        if (tile.TileType == TileTypes[2])
                            continue;

                        if (dist < rad)
                        {
                            if (rad - dist < MaxDitherDistance)
                            {
                                float chance = (rad - dist) / MaxDitherDistance;

                                if (WorldGen.genRand.NextFloat() <= chance && Main.tile[nPos.X, nPos.Y].HasTile)
                                {
                                    tile.ClearEverything();
                                    WorldGen.PlaceTile(nPos.X, nPos.Y, TileTypes[2], true);
                                }
                            }
                            else
                            {
                                tile.ClearEverything();
                                WorldGen.PlaceTile(nPos.X, nPos.Y, TileTypes[2], true);
                            }
                        }
                    }
                }
            }
        }
    }
}