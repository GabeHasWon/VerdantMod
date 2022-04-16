using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.World.Generation;
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

namespace Verdant.World
{
    ///Handles specific Verdant biome gen.
    public partial class VerdantWorld : ModWorld
    {
        private static int[] TileTypes { get => new int[] { ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>(), TileID.ChlorophyteBrick, ModContent.TileType<VerdantLightbulb>(), ModContent.TileType<LivingLushWood>() }; }
        private static int[] WallTypes { get => new int[] { ModContent.WallType<VerdantLeafWall_Unsafe>(), ModContent.WallType<LushSoilWall_Unsafe>(), ModContent.WallType<LushSoilWall_Unsafe>() }; }

        private const int MinRad = 70; //Minimum radius
        private const int MaxRad = 95; //Maximum radius

        public static Point VerdantCentre = new Point();
        public static Rectangle VerdantArea = new Rectangle(0, 0, 0, 0);

        private readonly List<GenCircle> VerdantCircles = new List<GenCircle>();

        public void VerdantGeneration(GenerationProgress p)
        {
            p.Message = "Growing plants...";

            mod.Logger.Info("World Seed: " + WorldGen._genRandSeed);
            mod.Logger.Info("Noise Seed: " + genNoise.Seed);

            Mod spirit = ModLoader.GetMod("SpiritMod");
            Mod calamity = ModLoader.GetMod("Calamity");

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
                    || WorldGen.UndergroundDesertLocation.Contains(VerdantCentre.X + FluffX, VerdantCentre.Y - FluffY) || WorldGen.UndergroundDesertLocation.Contains(VerdantCentre.X + FluffX, VerdantCentre.Y + FluffY))
                    continue;
                for (int i = VerdantCentre.X - (int)(FluffX * 1.2f); i < VerdantCentre.X + (FluffX * 1.2f); ++i) //Assume width
                {
                    for (int j = VerdantCentre.Y - 140; j < VerdantCentre.Y + 140; ++j) //Assume height
                    {
                        List<int> invalidTypes = new List<int>() { TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick, TileID.LihzahrdBrick, TileID.IceBlock, TileID.SnowBlock }; //Vanilla blacklist

                        if (spirit != null) //Spirit blacklist
                            invalidTypes.Add(spirit.TileType("BriarGrass"));
                        if (calamity != null) //Calamity blacklist
                            invalidTypes.Add(calamity.TileType("Navystone"));

                        if ((Framing.GetTileSafely(i, j).active() && invalidTypes.Any(x => Framing.GetTileSafely(i, j).type == x)))
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

            AddSurfaceVerdant();
        }

        private void AddSurfaceVerdant()
        {
            int top = Helper.FindDown(new Vector2(VerdantArea.Center.X * 16, 200));
            Point16 size = Point16.Zero;

            if (StructureHelper.Generator.GetDimensions("World/Structures/SurfaceTree", VerdantMod.Instance, ref size)) 
                StructureHelper.Generator.GenerateStructure("World/Structures/SurfaceTree", new Point16(VerdantArea.Center.X - (size.X / 2), top - size.Y + 12), VerdantMod.Instance);
        }

        public void VerdantCleanup(GenerationProgress p)
        {
            p.Message = "Trimming plants...";

            AddFlowerStructures();

            PlaceApotheosis();

            for (int i = VerdantArea.Right; i > VerdantArea.X; --i)
            {
                for (int j = VerdantArea.Bottom; j > VerdantArea.Y; --j)
                {
                    Main.tile[i, j].lava(false);

                    Tile t = Framing.GetTileSafely(i, j);
                    int[] vineAnchors = new int[] { ModContent.TileType<VerdantVine>(), ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<VerdantLeaves>() };
                    if (t.type == ModContent.TileType<VerdantVine>() && !vineAnchors.Contains(Framing.GetTileSafely(i, j - 1).type))
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
                    if (t.active() && invalidTypes.Contains(t.type))
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
            Point[] offsets = new Point[4] { new Point(4, 1), new Point(5, 1), new Point(3, 1), new Point(7, 0) }; //ruler in-game is ONE HIGHER on both planes
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
                    StructureHelper.Generator.GenerateMultistructureSpecific("World/Structures/Flowers", pos, mod, index);
                    positions.Add(pos.ToVector2());

                    if (!WorldGen.genRand.NextBool(6)) //NORMAL chests
                    {
                        bool c = GenHelper.PlaceChest(pos.X + offsets[index].X, pos.Y + offsets[index].Y + 1, ModContent.TileType<VerdantYellowPetalChest>(), new (int, int)[]
                        {
                            (ModContent.ItemType<VerdantStaff>(), 1), (ModContent.ItemType<Lightbloom>(), 1)
                        }, new (int, int)[] {
                            (ItemID.IronskinPotion, WorldGen.genRand.Next(1, 3)), (ItemID.ThornsPotion, WorldGen.genRand.Next(1, 3)), (ItemID.ThrowingKnife, WorldGen.genRand.Next(3, 7)),
                            (ModContent.ItemType<PinkPetal>(), WorldGen.genRand.Next(3, 7)), (ModContent.ItemType<RedPetal>(), WorldGen.genRand.Next(3, 7)), (ModContent.ItemType<Lightbulb>(), WorldGen.genRand.Next(1, 3)),
                            (ItemID.Dynamite, 1), (ItemID.Glowstick, WorldGen.genRand.Next(3, 8)), (ItemID.Glowstick, WorldGen.genRand.Next(3, 8)), (ItemID.Bomb, WorldGen.genRand.Next(2, 4)),
                            (ItemID.NightOwlPotion, WorldGen.genRand.Next(2, 4)), (ItemID.HealingPotion, WorldGen.genRand.Next(2, 4)), (ItemID.MoonglowSeeds, WorldGen.genRand.Next(2, 4)),
                            (ItemID.DaybloomSeeds, WorldGen.genRand.Next(2, 4)), (ItemID.BlinkrootSeeds, WorldGen.genRand.Next(2, 4))
                        }, true, WorldGen.genRand, WorldGen.genRand.Next(4, 7), 0, true);

                        if (!c)
                            mod.Logger.Warn("Failed to place Verdant Yellow Petal Chest.");
                    }
                    else //WAND chest
                    {
                        bool c = GenHelper.PlaceChest(pos.X + offsets[index].X, pos.Y + offsets[index].Y + 1, ModContent.TileType<VerdantYellowPetalChest>(), 0, false,
                            (ModContent.ItemType<LushLeafWand>(), 1), (ModContent.ItemType<PinkPetalWand>(), 1), (ModContent.ItemType<RedPetalWand>(), 1), (ModContent.ItemType<RedPetal>(), WorldGen.genRand.Next(19, 24)),
                            (ModContent.ItemType<PinkPetal>(), WorldGen.genRand.Next(19, 24)), (ModContent.ItemType<VerdantFlowerBulb>(), WorldGen.genRand.Next(12, 22)));

                        if (!c)
                            mod.Logger.Warn("Failed to place Verdant Yellow Petal Chest (wand).");
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
                Point p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                while (!TileHelper.ActiveType(p.X, p.Y, ModContent.TileType<LushSoil>()))
                    p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                WorldGen.TileRunner(p.X, p.Y, WorldGen.genRand.NextFloat(7, 15), WorldGen.genRand.Next(5, 15), TileID.Stone, false, 0, 0, false, true);
            }

            for (int i = 0; i < 10 * WorldSize; ++i) //Ores
            {
                Point p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                while (!TileHelper.ActiveType(p.X, p.Y, ModContent.TileType<LushSoil>()))
                    p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                WorldGen.TileRunner(p.X, p.Y, WorldGen.genRand.NextFloat(2, 8), WorldGen.genRand.Next(5, 15), TileID.Gold, false, 0, 0, false, true);
                p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                while (!TileHelper.ActiveType(p.X, p.Y, ModContent.TileType<LushSoil>()))
                    p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                WorldGen.TileRunner(p.X, p.Y, WorldGen.genRand.NextFloat(2, 7), WorldGen.genRand.Next(5, 15), TileID.Platinum, false, 0, 0, false, true);
            }
        }

        private void AddWater()
        {
            for (int i = 0; i < 26 * WorldSize; ++i)
            {
                Point p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                for (int j = -14; j < 14; ++j)
                {
                    for (int k = -14; k < 14; ++k)
                    {
                        Main.tile[p.X + j, p.Y + k].liquid = 255;
                        Main.tile[p.X + j, p.Y + k].liquidType(0);
                    }
                }
            }
        }

        private void Vines()
        {
            for (int i = 0; i < 220 * WorldSize; ++i)
            {
                Point rP = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                Point adj = TileHelper.GetRandomOpenAdjacent(rP.X, rP.Y);
                while (adj == new Point(-2, -2) || adj == new Point(0, -1) || adj == new Point(0, 1))
                {
                    rP = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
                    adj = TileHelper.GetRandomOpenAdjacent(rP.X, rP.Y);
                }
                Tile tile = Framing.GetTileSafely(rP.X, rP.Y);

                if (tile.type == ModContent.TileType<VerdantGrassLeaves>() || tile.type == ModContent.TileType<LushSoil>() || tile.type == ModContent.TileType<LivingLushWood>())
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

        private void AddPlants()
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
                        if (!Framing.GetTileSafely(i, j + 1).active() && !Framing.GetTileSafely(i, j + 1).bottomSlope() && WorldGen.genRand.Next(5) <= 2)
                        {
                            int length = WorldGen.genRand.Next(2, 22);
                            bool strong = WorldGen.genRand.Next(10) == 0;

                            for (int l = 1; l < length; ++l)
                            {
                                if (Framing.GetTileSafely(i, j + l + 1).active())
                                    break;

                                WorldGen.KillTile(i, j + l, false, false, true); //please
                                WorldGen.PlaceTile(i, j + l, strong ? ModContent.TileType<VerdantStrongVine>() : ModContent.TileType<VerdantVine>(), true, true);
                                Framing.GetTileSafely(i, j + l).type = (ushort)(strong ? ModContent.TileType<VerdantStrongVine>() : ModContent.TileType<VerdantVine>());

                                if (strong)
                                    Framing.GetTileSafely(i, j + l).frameY = (short)(Main.rand.Next(4) * 18);
                            }
                            continue;
                        }
                    }

                    //lightbulb
                    bool doPlace = Helper.AreaClear(i, j - 2, 2, 2) && TileHelper.ActiveTypeNoTopSlope(i, j, ModContent.TileType<VerdantGrassLeaves>()) && TileHelper.ActiveTypeNoTopSlope(i + 1, j, ModContent.TileType<VerdantGrassLeaves>());
                    if (doPlace && WorldGen.genRand.Next(11) == 0)
                    {
                        WorldGen.PlaceTile(i, j - 2, ModContent.TileType<VerdantLightbulb>(), true, false, -1, WorldGen.genRand.Next(3));
                        continue;
                    }

                    if (!Framing.GetTileSafely(i, j - 1).active() && !TileHelper.ActiveTypeNoTopSlope(i, j, ModContent.TileType<VerdantGrassLeaves>()) && WorldGen.genRand.Next(5) >= 1)
                    {
                        int type = !Main.rand.NextBool(1) ? ModContent.TileType<VerdantDecor1x1>() : ModContent.TileType<VerdantDecor1x1NoCut>();
                        WorldGen.PlaceTile(i, j - 1, type, true, false, -1, WorldGen.genRand.Next(7));
                        continue;
                    }

                    //ground decor 2x1
                    doPlace = !Framing.GetTileSafely(i, j - 1).active() && TileHelper.ActiveTypeNoTopSlope(i, j, ModContent.TileType<VerdantGrassLeaves>()) &&
                        !Framing.GetTileSafely(i + 1, j - 1).active() && TileHelper.ActiveTypeNoTopSlope(i + 1, j, ModContent.TileType<VerdantGrassLeaves>());
                    if (doPlace && WorldGen.genRand.Next(2) == 0)
                    {
                        GenHelper.PlaceMultitile(new Point(i, j - 1), ModContent.TileType<VerdantDecor2x1>(), WorldGen.genRand.Next(7));
                        continue;
                    }

                    //flower wall 2x2
                    doPlace = Helper.AreaClear(i, j, 2, 2) && Helper.WalledSquare(i, j, 2, 2) && Helper.WalledSquareType(i, j, 2, 2, WallTypes[0]);
                    if (doPlace && WorldGen.genRand.Next(42) == 0)
                    {
                        GenHelper.PlaceMultitile(new Point(i, j), WorldGen.genRand.Next(13) == 0 ? ModContent.TileType<MountedLightbulb_2x2>() : ModContent.TileType<Flower_2x2>(), WorldGen.genRand.Next(4));
                        continue;
                    }

                    //flower wall 3x3
                    doPlace = Helper.AreaClear(i, j, 3, 3) && Helper.WalledSquare(i, j, 3, 3) && Helper.WalledSquareType(i, j, 3, 3, WallTypes[0]);
                    if (doPlace && WorldGen.genRand.Next(68) == 0)
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
            VerdantCircles.Clear();
            for (int i = 0; i < repeats; ++i)
            {
                int x = (int)MathHelper.Lerp(VerdantArea.X + 50, VerdantArea.Right - 50,  i / repeats);
                int y = VerdantArea.Center.Y - WorldGen.genRand.Next(-20, 20);

                VerdantCircles.Add(new GenCircle(50, new Point(x, y)));
            }

            for (int i = 0; i < VerdantCircles.Count; ++i)
                VerdantCircles[i].Gen();
        }

        private void CleanForCaves()
        {
            TunnelSpice();

            //Caves
            genNoise.Seed = WorldGen._genRandSeed;
            genNoise.Frequency = 0.05f;
            genNoise.NoiseType = FastNoise.NoiseTypes.CubicFractal; //Sets noise to proper type
            genNoise.FractalType = FastNoise.FractalTypes.Billow;

            for (int i = VerdantCentre.X - Main.maxTilesX / 6; i < VerdantCentre.X + Main.maxTilesX / 6; ++i)
            {
                if (i < 2) i = 2;
                if (i > Main.maxTilesX - 2) break;

                for (int j = VerdantCentre.Y - Main.maxTilesY / 6; j < VerdantCentre.Y + Main.maxTilesY / 6; ++j)
                {
                    if (j < 2) j = 2;
                    if (j > Main.maxTilesY - 2) break;

                    Tile t = Framing.GetTileSafely(i, j);
                    if (t.active() && t.type == TileTypes[2])
                    {
                        float n = genNoise.GetNoise(i, j);
                        t.ClearTile();
                        if (n < -0.67f) { }
                        else if (n < -0.57f) WorldGen.PlaceTile(i, j, TileTypes[0]);
                        else WorldGen.PlaceTile(i, j, TileTypes[1]);

                        if (n < -0.85f) WorldGen.KillWall(i, j, false);
                        else if (n < -0.52f) WorldGen.PlaceWall(i, j, WallTypes[0]);
                        else WorldGen.PlaceWall(i, j, WallTypes[1]);
                    }
                }
            }

            //Roots
            genNoise.Seed = WorldGen._genRandSeed;
            genNoise.Frequency = 0.014f;
            genNoise.NoiseType = FastNoise.NoiseTypes.ValueFractal;
            genNoise.FractalType = FastNoise.FractalTypes.Billow;
            genNoise.InterpolationMethod = FastNoise.Interp.Quintic;

            for (int i = VerdantCentre.X - Main.maxTilesX / 6; i < VerdantCentre.X + Main.maxTilesX / 6; ++i)
            {
                for (int j = VerdantCentre.Y - Main.maxTilesY / 6; j < VerdantCentre.Y + Main.maxTilesY / 6; ++j)
                {
                    Tile t = Framing.GetTileSafely(i, j);
                    float n = genNoise.GetNoise(i, j);
                    if (t.wall == WallTypes[0] && n < -0.36f)
                        GenHelper.ReplaceWall(new Point(i, j), WallTypes[2]);

                    if (n < -0.68f && TileTypes.Any(x => x == t.type) && t.type != TileTypes[0] && t.active())
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
                        if (Framing.GetTileSafely(position.X + j, position.Y + k).active())
                        {
                            if (TileTypes.Contains(Framing.GetTileSafely(position.X + j, position.Y + k).type))
                                ver = true;
                            else
                                non = true;
                        }
                    }
                }

                if (!ver || !non)
                {
                    if (WorldGen.genRand.Next(9) == 0) runners++;
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
                    if (TileHelper.ActiveType(i, j, ModContent.TileType<VerdantLillie>()) && Framing.GetTileSafely(i, j).liquid < 155)
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
        private struct GenCircle
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
                int constSiz = (int)(rad * 3f); //Don't change this
                bool changeRad = true;

                for (int i = -(int)(constSiz / 2f); i < (int)(constSiz / 2f); ++i)
                {
                    for (int j = -(int)(constSiz / 2f); j < (int)(constSiz / 2f); ++j)
                    {
                        Point nPos = new Point(pos.X + i, pos.Y + j);
                        float dist = Vector2.Distance(nPos.ToVector2(), pos.ToVector2());
                        Tile tile = Framing.GetTileSafely(nPos.X, nPos.Y);
                        if (tile.type == TileTypes[2])
                            continue;
                        if ((nPos == pos || dist < rad + 12) && tile.type != TileTypes[2])
                        {
                            if (dist < rad)
                            {
                                tile.ClearEverything();
                                WorldGen.PlaceTile(nPos.X, nPos.Y, TileTypes[2], true);
                                changeRad = true;
                            }
                            else
                            {
                                int off = (int)dist - (rad + 12);
                                float chance = off / 12f;
                                if (WorldGen.genRand.Next(99) * 0.01f < chance && tile.active() && tile.type != TileTypes[2])
                                    tile.type = (ushort)TileTypes[1];
                            }
                        }
                        else
                        {
                            if (changeRad)
                            {
                                rad += WorldGen.genRand.Next(2) == 0 ? -1 : 1;
                                changeRad = false;
                            }
                            if (rad < MinRad * WorldSize)
                                rad = (int)(MinRad * WorldSize);
                            if (rad > MaxRad * WorldSize)
                                rad = (int)(MaxRad * WorldSize);
                        }
                    }
                }
            }
        }
    }
}