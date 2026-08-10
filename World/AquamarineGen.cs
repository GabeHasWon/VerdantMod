using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Verdant.Tiles;
using Verdant.Tiles.Verdant.Basic;
using Verdant.Tiles.Verdant.Basic.Aquamarine;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Basic.Puff;
using Verdant.Walls;

namespace Verdant.World;

internal class AquamarineGen
{
    private enum GroundedType
    {
        Single,
        DoubleWide
    }

    public static void Gen(GenerationProgress progress, GameConfiguration config)
    {
        int repeats = (int)(10 * VerdantGenSystem.WorldSize * BackslateConfiguration.BackslateMicrobiomeModifier);
        int tries = 0;

        for (int i = 0; i < repeats; i++)
        {
            progress.Value = (float)i / repeats;

            int x;
            int y;
            Tile tile;

            do
            {
                x = WorldGen.genRand.Next(VerdantGenSystem.VerdantArea.Left, VerdantGenSystem.VerdantArea.Right);
                y = WorldGen.genRand.Next(VerdantGenSystem.VerdantArea.Top, VerdantGenSystem.VerdantArea.Bottom);
                tile = Main.tile[x, y];
                tries++;
            } while (tries < 10_000 && tile.TileType != ModContent.TileType<VerdantGrassLeaves>() || WorldGen.SolidOrSlopedTile(tile));

            if (tries >= 10_000)
                return;

            SpawnBackslateMinibiome(x, y);
        }
    }

    public static void SpawnBackslateMinibiome(int x, int y)
    {
        int biomeWidth = BackslateConfiguration.BackslateWidth;

        for (int i = 0; i < (2 + VerdantGenSystem.WorldSize) * BackslateConfiguration.BackslateSpam; ++i)
        {
            int dX = x + WorldGen.genRand.Next(-biomeWidth / 2, biomeWidth / 2);
            int dY = y + WorldGen.genRand.Next(-biomeWidth / 2, biomeWidth / 2);
            WorldGen.TileRunner(dX, dY, 3 * BackslateConfiguration.BackslateSizeModifier, 11, ModContent.TileType<BackslateTile>(), false, 0, 0, false, true);
        }

        Dictionary<GroundedType, List<Point>> grounds = new()
        {
            { GroundedType.DoubleWide, new List<Point>() },
            { GroundedType.Single, new List<Point>() }
        };

        int[] killables = new int[] { ModContent.TileType<VerdantDecor1x1>(), ModContent.TileType<VerdantDecor1x1NoCut>(), ModContent.TileType<VerdantDecor1x2>(),
            ModContent.TileType<VerdantDecor1x3>(), ModContent.TileType<VerdantDecor2x1>(), ModContent.TileType<VerdantDecor2x2>(), ModContent.TileType<PuffDecor1x1>(),
            ModContent.TileType<PuffDecor1x2>() };

        bool ValidForReplacement(int i, int j) => !WorldGen.SolidOrSlopedTile(i, j - 1) && (!Main.tile[i, j].HasTile || TileHelper.ActiveType(i, j, killables));

        for (int j = y - biomeWidth; j < y + biomeWidth; ++j)
        {
            for (int i = x - biomeWidth; i < x + biomeWidth; ++i)
            {
                bool singleValid = WorldGen.SolidOrSlopedTile(i, j);

                if (singleValid && WorldGen.SolidOrSlopedTile(i + 1, j) && ValidForReplacement(i, j - 1) && ValidForReplacement(i + 1, j - 1))
                {
                    grounds[GroundedType.DoubleWide].Add(new Point(i, j));
                    i++; //Skip forward
                }
                else if (singleValid)
                    grounds[GroundedType.Single].Add(new Point(i, j));

                WorldGen.PlaceLiquid(i, j, (byte)LiquidID.Water, 175);

                if (singleValid)
                {
                    TryPlaceWalls(i, j);
                    TryPlaceAquamarine(i, j);
                }
            }
        }

        SpamBushes(grounds[GroundedType.DoubleWide]);
    }

    private static void TryPlaceAquamarine(int i, int j)
    {
        static void TryPlace(int x, int y)
        {
            if ((!Main.tile[x, y].HasTile || Main.tileCut[Main.tile[x, y].TileType]) && WorldGen.genRand.NextBool(BackslateConfiguration.AquamarineChance))
            {
                WorldGen.KillTile(x, y);
                WorldGen.PlaceTile(x, y, ModContent.TileType<AquamarineTile>(), true, true);
            }
        }

        TryPlace(i + 1, j);
        TryPlace(i - 1, j);
        TryPlace(i, j + 1);
        TryPlace(i, j - 1);
    }

    private static void TryPlaceWalls(int i, int j)
    {
        if (!WorldGen.SolidOrSlopedTile(i, j - 1) && !WorldGen.genRand.NextBool(BackslateConfiguration.WallChance))
        {
            int y = j - 1;
            int cutoffHeight = WorldGen.genRand.Next(4, 9);
            int height = WorldGen.genRand.Next(13, 19);

            while (!WorldGen.SolidOrSlopedTile(i, y - 1))
            {
                WorldGen.KillWall(i, y);

                if (cutoffHeight >= 0)
                {
                    bool flag = WorldGen.genRand.NextBool(BackslateConfiguration.BubblingWallChance);
                    WorldGen.PlaceWall(i, y, flag ? ModContent.WallType<BubblingWall_Unsafe>() : ModContent.WallType<BackslateWall_Unsafe>());
                }

                y--;
                height--;
                cutoffHeight--;

                if (height <= 0)
                    break;
            }
        }
    }

    private static void SpamBushes(List<Point> grounds)
    {
        for (int i = 0; i < grounds.Count; ++i)
        {
            Point item = grounds[i];

            if (WorldGen.genRand.NextBool(BackslateConfiguration.BushChance))
            {
                WorldGen.KillTile(item.X, item.Y - 1, false, false, true);
                WorldGen.KillTile(item.X + 1, item.Y - 1, false, false, true);
                int height = WorldGen.genRand.Next(5, 13);

                for (int j = 1; j < height; ++j)
                    WorldGen.PlaceObject(item.X, item.Y - j, ModContent.TileType<WaterberryBush>(), true);

                grounds.RemoveAt(i);
                i--;
            }
        }
    }

    internal static void SpamGems(GenerationProgress progress, GameConfiguration configuration)
    {
        float repeats = BackslateConfiguration.RandomAquamarine * VerdantGenSystem.WorldSize;

        for (int i = 0; i < repeats; ++i) //Aquamarine
        {
            Point p = new(WorldGen.genRand.Next(40, Main.maxTilesX - 40), WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 200));
            while (!TileHelper.ActiveType(p.X, p.Y, TileID.Stone, TileID.ClayBlock))
                p = new(WorldGen.genRand.Next(40, Main.maxTilesX - 40), WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 200));

            float str = WorldGen.genRand.NextFloat(2, 5) * BackslateConfiguration.AquamarineChunkSize;
            WorldGen.TileRunner(p.X, p.Y, str, WorldGen.genRand.Next(5, 15), ModContent.TileType<EmbeddedStoneAquamarine>(), false, 0, 0, false, true);

            progress.Value = i / repeats;
        }
    }
}
