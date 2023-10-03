using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Verdant.Tiles;
using Verdant.Tiles.Verdant.Basic;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Basic.Puff;
using Verdant.Tiles.Verdant.Mounted;
using Verdant.Tiles.Verdant.Trees;
using Verdant.Walls;

namespace Verdant.World;

public partial class VerdantGenSystem
{
    private static void Vines()
    {
        for (int i = 0; i < 240 * WorldSize; ++i)
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
        LoopTrees();

        for (int i = VerdantArea.X; i < VerdantArea.Right; ++i)
        {
            for (int j = VerdantArea.Y; j < VerdantArea.Bottom; ++j)
            {
                bool puff = VerdantGrassLeaves.CheckPuffMicrobiome(i, j, 1.25f);

                if (TileHelper.ActiveType(i, j, ModContent.TileType<VerdantGrassLeaves>()))
                {
                    //Vines
                    if (!Framing.GetTileSafely(i, j + 1).HasTile && !Framing.GetTileSafely(i, j + 1).BottomSlope && WorldGen.genRand.Next(5) <= 2)
                    {
                        int length = WorldGen.genRand.Next(4, 20);
                        bool strong = WorldGen.genRand.NextBool(10);

                        int type = strong ? ModContent.TileType<VerdantStrongVine>() : ModContent.TileType<VerdantVine>();
                        if (puff)
                            type = ModContent.TileType<PuffVine>();

                        for (int l = 1; l < length; ++l)
                        {
                            if (Framing.GetTileSafely(i, j + l + 1).HasTile)
                                break;

                            WorldGen.KillTile(i, j + l, false, false, true);
                            WorldGen.PlaceTile(i, j + l, type, true, true);
                            Framing.GetTileSafely(i, j + l).TileType = (ushort)type;

                            if (strong)
                                Framing.GetTileSafely(i, j + l).TileFrameY = (short)(WorldGen.genRand.Next(4) * 18);
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

                //weeping bud
                doPlace = Helper.AreaClear(i - 1, j + 1, 3, 2) && TileHelper.ActiveTypeNoBottomSlope(i, j, ModContent.TileType<VerdantGrassLeaves>());
                if (doPlace && WorldGen.genRand.NextBool(32))
                {
                    WorldGen.PlaceTile(i, j + 1, ModContent.TileType<WaterPlant>(), true, true);
                    continue;
                }

                //beehive
                doPlace = Helper.AreaClear(i, j - 2, 2, 2) && TileHelper.ActiveTypeNoTopSlope(i, j, ModContent.TileType<VerdantGrassLeaves>()) && TileHelper.ActiveTypeNoTopSlope(i + 1, j, ModContent.TileType<VerdantGrassLeaves>());
                if (doPlace && WorldGen.genRand.NextBool(40))
                {
                    WorldGen.PlaceTile(i, j - 2, ModContent.TileType<Beehive>(), true, false);
                    continue;
                }

                if (puff && PuffDecor(i, j))
                    continue;
                else if (!puff && NormalDecor(i, j))
                    continue;

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

    private static void LoopTrees()
    {
        for (int i = VerdantArea.X; i < VerdantArea.Right; ++i)
        {
            for (int j = VerdantArea.Y; j < VerdantArea.Bottom; ++j) //Loop explicitly for trees & puffs so they get all the spawns they need
            {
                //Trees
                if (TileHelper.ActiveType(i, j, ModContent.TileType<VerdantGrassLeaves>()))
                {
                    int minHeight = 0;

                    for (int k = 1; k < 13; ++k)
                        if (!WorldGen.TileEmpty(i, j - k))
                            minHeight = k;

                    if (minHeight > 6 && WorldGen.genRand.NextBool(16))
                        VerdantTree.Spawn(i, j - 1, -1, WorldGen.genRand, 6, minHeight, false, -1, false);
                }

                //Puffs
                bool doPlace = Helper.AreaClear(i, j, 2, 3) && TileHelper.ActiveTypeNoTopSlope(i, j + 3, ModContent.TileType<VerdantGrassLeaves>()) &&
                    TileHelper.ActiveTypeNoTopSlope(i + 1, j + 3, ModContent.TileType<VerdantGrassLeaves>());

                if (doPlace && WorldGen.genRand.NextBool(60))
                {
                    WorldGen.PlaceObject(i, j + 1, ModContent.TileType<BigPuff>(), true);

                    int pickipuffs = WorldGen.genRand.Next(1, 4);
                    for (int k = 0; k < pickipuffs; ++k)
                    {
                        int x = i - WorldGen.genRand.Next(-10, 11);
                        int y = Helper.FindUpWithType(new Point(x, j), ModContent.TileType<VerdantGrassLeaves>());

                        if (y != -1)
                            ModContent.GetInstance<Tiles.TileEntities.Puff.Pickipuff>().Place(x, y);
                    }
                }
            }
        }
    }

    private static bool NormalDecor(int i, int j)
    {
        //decor 1x1
        if (!Framing.GetTileSafely(i, j - 1).HasTile && TileHelper.ActiveTypeNoTopSlope(i, j, ModContent.TileType<VerdantGrassLeaves>()) && WorldGen.genRand.Next(5) >= 1)
        {
            int type = WorldGen.genRand.NextBool(2) ? ModContent.TileType<VerdantDecor1x1>() : ModContent.TileType<VerdantDecor1x1NoCut>();
            WorldGen.PlaceTile(i, j - 1, type, true, true, style: WorldGen.genRand.Next(7));
            return true;
        }

        //ground decor 2x1
        bool doPlace = !Framing.GetTileSafely(i, j - 1).HasTile && TileHelper.ActiveTypeNoTopSlope(i, j, ModContent.TileType<VerdantGrassLeaves>()) &&
            !Framing.GetTileSafely(i + 1, j - 1).HasTile && TileHelper.ActiveTypeNoTopSlope(i + 1, j, ModContent.TileType<VerdantGrassLeaves>());
        if (doPlace && WorldGen.genRand.NextBool(2))
        {
            GenHelper.PlaceMultitile(new Point(i, j - 1), ModContent.TileType<VerdantDecor2x1>(), WorldGen.genRand.Next(7));
            return true;
        }
        return false;
    }

    private static bool PuffDecor(int i, int j)
    {
        //decor 1x1
        if (!Framing.GetTileSafely(i, j - 1).HasTile && TileHelper.ActiveTypeNoTopSlope(i, j, ModContent.TileType<VerdantGrassLeaves>()) && WorldGen.genRand.Next(8) >= 1)
        {
            WorldGen.PlaceTile(i, j - 1, ModContent.TileType<PuffDecor1x1>(), true, false, -1, WorldGen.genRand.Next(7));
            return true;
        }
        return false;
    }
}
