﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Verdant.Tiles;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Decor;
using Verdant.Tiles.Verdant.Misc;
using Verdant.Tiles.Verdant.Trees;

namespace Verdant.World;

public partial class VerdantGenSystem
{
    public void VerdantCleanup(GenerationProgress p, GameConfiguration config)
    {
        p.Message = "Trimming plants...";

        AddFlowerStructures();
        p.Value = 0.5f;

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

        ClearOrphans();
    }

    private void ClearOrphans()
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

    readonly static int[] InvalidTypes = new int[] { TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick, TileID.LihzahrdBrick };
    readonly static int[] InvalidWalls = new int[] { WallID.BlueDungeonSlabUnsafe, WallID.BlueDungeonUnsafe, WallID.BlueDungeonTileUnsafe, WallID.GreenDungeonSlabUnsafe, WallID.GreenDungeonTileUnsafe,
            WallID.GreenDungeonUnsafe, WallID.PinkDungeonUnsafe, WallID.PinkDungeonTileUnsafe, WallID.PinkDungeonSlabUnsafe };

    public override void PostWorldGen()
    {
        Point apothPos = new(VerdantArea.Center.X - 10, VerdantArea.Center.Y - 4);
        int side = WorldGen.genRand.NextBool(2) ? -1 : 1;
        bool triedOneSide = false;

    redo:
        for (int i = 0; i < 20; ++i)
        {
            for (int j = 0; j < 18; ++j)
            {
                Tile t = Framing.GetTileSafely(apothPos.X + i, apothPos.Y + j);
                if (t.HasTile && (InvalidTypes.Contains(t.TileType) || InvalidWalls.Contains(t.WallType)))
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

        ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation = new Point16(apothPos.X + 8, apothPos.Y + 4);
        StructureHelper.Generator.GenerateStructure("World/Structures/Apotheosis", new Point16(apothPos.X, apothPos.Y), VerdantMod.Instance);

    redoAgain:
        side = WorldGen.genRand.NextBool(2) ? -1 : 1;
        int studyID = WorldGen.genRand.Next(2);
        Point16 size = new();
        bool foundGround = false;
        int[] valids = new int[] { ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>() };
        Point studyLoc = new(VerdantArea.Left + (int)(WorldGen.genRand.Next(20, 80) * WorldSize), WorldGen.genRand.Next(VerdantArea.Top, VerdantArea.Bottom));

        if (side == 1)
            studyLoc.X = VerdantArea.Right - (int)(WorldGen.genRand.Next(20, 80) * WorldSize);

        StructureHelper.Generator.GetDimensions("World/Structures/Study" + studyID, VerdantMod.Instance, ref size);

        for (int i = 0; i < size.X; ++i)
        {
            for (int j = 0; j < size.Y; ++j)
            {
                Tile t = Framing.GetTileSafely(studyLoc.X + i, studyLoc.Y + j);

                if (t.HasTile && (InvalidTypes.Contains(t.TileType) || InvalidWalls.Contains(t.WallType)))
                    goto redoAgain;

                if (valids.Contains(t.TileType))
                    foundGround = true;
            }
        }

        if (!foundGround)
            goto redoAgain;

        StructureHelper.Generator.GenerateStructure("World/Structures/Study" + studyID, new Point16(studyLoc.X, studyLoc.Y), VerdantMod.Instance);
        ReplaceBooks(studyLoc, size);

    redoAgainAgain:
        Point pos = new(VerdantArea.Left + (int)(WorldGen.genRand.Next(20, 80) * WorldSize), WorldGen.genRand.Next(VerdantArea.Top, VerdantArea.Bottom));

        if (side == 0)
            studyLoc.X = VerdantArea.Right - (int)(WorldGen.genRand.Next(20, 80) * WorldSize);

        int groundCount = Helper.TileRectangle(pos.X, pos.Y + 6, 6, 5, ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>());
        if (Helper.NoTileRectangle(pos.X, pos.Y, 6, 6) > 20 && groundCount > 25)
            StructureHelper.Generator.GenerateStructure("World/Structures/SnailStatue", new Point16(pos.X, pos.Y), VerdantMod.Instance);
        else
            goto redoAgainAgain;
    }

    private static void ReplaceBooks(Point studyLoc, Point16 size)
    {
        bool placeRock = true;

        while (true)
        {
            int x = studyLoc.X + WorldGen.genRand.Next(size.X);
            int y = studyLoc.Y + WorldGen.genRand.Next(size.Y);

            if (TileHelper.ActiveType(x, y, ModContent.TileType<ResearchBooks>()))
            {
                Tile tile = Main.tile[x, y];
                tile.TileType = (ushort)ModContent.TileType<SpecialBooks>();
                tile.TileFrameX = (short)((placeRock ? 108 : 72) + (WorldGen.genRand.NextBool() ? 18 : 0));

                if (!placeRock)
                    break;
                else
                    placeRock = false;
            }
        }
    }

    private void AddFlowerStructures()
    {
        Point[] offsets = new Point[3] { new Point(7, -1), new Point(3, 0), new Point(3, 0) }; //ruler in-game is ONE HIGHER on both planes

        var list = InvalidTypes.ToList();
        list.Add(ModContent.TileType<Apotheosis>());
        int[] invalids = list.ToArray();

        int[] valids = new int[] { ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>() };

        List<Vector2> positions = new() { new Vector2(VerdantArea.Center.X - 10, VerdantArea.Center.Y - 4) }; //So I don't overlap with the Apotheosis
        int attempts = 0;

        for (int i = 0; i < 8 * WorldSize; ++i)
        {
            int index = WorldGen.genRand.Next(offsets.Length);
            Point16 pos = new(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));

            bool notNear = !positions.Any(x => Vector2.DistanceSquared(x, pos.ToVector2()) < 20 * 20);

            if (notNear && Helper.TileRectangle(pos.X, pos.Y, 20, 10, valids) > 4 && Helper.TileRectangle(pos.X, pos.Y, 20, 10, invalids) <= 0 && Helper.NoTileRectangle(pos.X, pos.Y, 20, 10) > 40)
            {
                StructureHelper.Generator.GenerateMultistructureSpecific("World/Structures/Flowers", pos, Mod, index);
                positions.Add(pos.ToVector2());
            }
            else
            {
                i--;

                if (attempts++ > 500)
                    return;
                continue;
            }
        }
    }
}
