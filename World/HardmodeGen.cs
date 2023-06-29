using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Tiled;
using Verdant.Tiles;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Decor;
using Verdant.Tiles.Verdant.Trees;

namespace Verdant.World;

internal class HardmodeGen : ModSystem
{
    public override void ModifyHardmodeTasks(List<GenPass> list)
    {
        list.Add(new PassLegacy("Verdant Hardmode", HardmodeTasks));
    }

    public void HardmodeTasks(GenerationProgress p, GameConfiguration config)
    {
        for (int x = 40; x <= Main.maxTilesX - 40; ++x)
        {
            for (int y = 40; y <= Main.maxTilesY - 40; ++y)
            {
                Tile tile = Main.tile[x, y];

                if (!tile.HasTile)
                    continue;

                Replace(x, y, ModContent.TileType<VerdantVine>(), ModContent.TileType<LightbulbVine>());
                Replace(x, y, ModContent.TileType<Apotheosis>(), ModContent.TileType<HardmodeApotheosis>());
            }
        }

        GenerateMysteria();
    }

    private static void GenerateMysteria()
    {
        Point16? apothLoc = ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation;

        if (apothLoc is null)
        {
            VerdantMod.Instance.Logger.Warn("Apotheosis could not be found! Mysteria hardmode gen skipped.");
            return;
        }

        Point16 pos = apothLoc.Value;
        int halfWidth = (int)(600 * VerdantGenSystem.WorldSize) / 2;
        int repeats = 0;

        for (int i = 0; i < 4; ++i)
        {
            repeats++;
            int x = pos.X + WorldGen.genRand.Next(halfWidth * 2) - halfWidth;
            int y = pos.Y - 80 + WorldGen.genRand.Next(160);
            bool ground = TileHelper.ActiveType(x, y + 1, ModContent.TileType<VerdantGrassLeaves>());
            bool safeWall = Main.tile[x, y].WallType > WallID.None && Main.wallHouse[Main.tile[x, y].WallType];

            if (repeats > 2000000) //Extremely unlikely fallback
            {
                if (!ground)
                {
                    i--;
                    continue;
                }

                int length = WorldGen.genRand.Next(4, 7);

                for (int j = 0; j < length; ++j)
                {
                    if (WorldGen.SolidOrSlopedTile(x, y - j - 2))
                        length = j + 1;

                    int type = j == length - 1 ? ModContent.TileType<MysteriaTreeTop>() : ModContent.TileType<MysteriaTree>();
                    WorldGen.PlaceTile(x, y - j, type);
                }
                continue;
            }

            if (!ground || safeWall || !MysteriaTree.Generate(x, y, 0, WorldGen.genRand))
                i--;
            else
                AddMysteriaDrapes(x, y);
        }

        WorldGen.BroadcastText(Terraria.Localization.NetworkText.FromLiteral("There's some rumbling coming from the Verdant..."), Color.DarkGreen);
    }

    private static void AddMysteriaDrapes(int x, int y)
    {
        for (int i = x - 20; i < x + 20; ++i)
        {
            for (int j = y - 20; j < y + 20; ++j)
            {
                if (TileHelper.ActiveType(i, j, ModContent.TileType<VerdantGrassLeaves>()) && !WorldGen.SolidOrSlopedTile(i, j + 1) && WorldGen.genRand.NextBool(4))
                {
                    var drape = new MysteriaDrapes(new Point(i, j));
                    int off = 1;

                    while (!WorldGen.SolidOrSlopedTile(i, j + off))
                    {
                        if (!WorldGen.genRand.NextBool(5))
                            drape.Grow();

                        off++;
                    }

                    ForegroundManager.AddItem(drape, true);
                }
            }
        }
    }

    private static void Replace(int x, int y, int replace, int newType)
    {
        Tile tile = Main.tile[x, y];

        if (tile.TileType == replace)
            tile.TileType = (ushort)newType;
    }
}
