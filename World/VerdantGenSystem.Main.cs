﻿using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.WorldBuilding;
using Verdant.Noise;
using Verdant.Walls;
using Verdant.Tiles.Verdant.Trees;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles;
using Terraria.IO;
using System;
using Verdant.Tiles.Verdant.Basic.Aquamarine;
using System.Diagnostics;

namespace Verdant.World;

///Handles specific Verdant biome gen.
public partial class VerdantGenSystem : ModSystem
{
    public static float WorldSize { get => Main.maxTilesX / 4200f; }

    private static int[] TileTypes => new int[] { ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>(), TileID.ChlorophyteBrick, ModContent.TileType<VerdantLightbulb>(), ModContent.TileType<LivingLushWood>() };
    private static int[] WallTypes  => new int[] { ModContent.WallType<VerdantLeafWall_Unsafe>(), ModContent.WallType<LushSoilWall_Unsafe>(), ModContent.WallType<LivingLushWoodWall_Unsafe>() };

    internal static Rectangle VerdantArea = new(0, 0, 0, 0);

    private readonly List<GenCircle> VerdantCircles = new();

    internal Point16? apotheosisLocation = null;

    public void VerdantGeneration(GenerationProgress p, GameConfiguration config)
    {
        p.Message = "Growing plants...";

        Mod.Logger.Info("World Seed: " + WorldGen._genRandSeed + "\nNoise Seed: " + VerdantSystem.genNoise.Seed);

        static bool IsInvalidCenterX(int x)
        {
            if (ModContent.GetInstance<VerdantClientConfig>().JungleSpawn)
            {
                for (int y = 200; y < Main.maxTilesY - 200; ++y)
                    if (TileHelper.ActiveType(x, y, TileID.JungleGrass))
                        return false;

                return true;
            }

            return Math.Abs(x - (Main.maxTilesX / 2)) < 220;
        }

        static int GetCenterX()
        {
            if (WorldGen.remixWorldGen)
                return WorldGen.genRand.Next(Main.maxTilesX / 3, (int)(Main.maxTilesX / 3f * 2));

            int x;

            do
            {
                x = WorldGen.genRand.Next(Main.maxTilesX / 5, (int)(Main.maxTilesX / 1.25f));
            } while (IsInvalidCenterX(x));
            return x;
        }

        static int GetCenterY() => ModContent.GetInstance<VerdantClientConfig>().JungleSpawn ?
            WorldGen.genRand.Next((int)(Main.maxTilesY / 2.7f), (int)(Main.maxTilesY / 1.9f)) :
            WorldGen.genRand.Next((int)(Main.maxTilesY / 2.1f), (int)(Main.maxTilesY / 1.65f));

        Point center = new(GetCenterX(), GetCenterY());

        int FluffX = (int)(230 * WorldSize);
        int FluffY = (int)(130 * WorldSize);

        int total = 0;
        while (true) //Find valid position for biome
        {
        reset:
            center = new Point(GetCenterX(), GetCenterY());
            total = 0;
            if (GenVars.UndergroundDesertLocation.Contains(center.X - FluffX, center.Y - FluffY) || GenVars.UndergroundDesertLocation.Contains(center.X - FluffX, center.Y + FluffY)
                || GenVars.UndergroundDesertLocation.Contains(center.X + FluffX, center.Y - FluffY) || GenVars.UndergroundDesertLocation.Contains(center.X + FluffX, center.Y + FluffY)
                || GenVars.UndergroundDesertLocation.Contains(center))
                continue;
            for (int i = center.X - (int)(FluffX * 1.2f); i < center.X + (FluffX * 1.2f); ++i) //Assume width
            {
                for (int j = center.Y - 140; j < center.Y + 140; ++j) //Assume height
                {
                    List<int> invalidTypes = new() { TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick, TileID.LihzahrdBrick, TileID.IceBlock, TileID.SnowBlock }; //Vanilla blacklist

                    if (ModLoader.TryGetMod("SpiritMod", out Mod spirit)) //Spirit blacklist
                        invalidTypes.Add(spirit.Find<ModTile>("BriarGrass").Type);
                    if (ModLoader.TryGetMod("CalamityMod", out Mod calamity)) //Calamity blacklist
                        invalidTypes.Add(calamity.Find<ModTile>("Navystone").Type);

                    if ((Framing.GetTileSafely(i, j).HasTile && invalidTypes.Any(x => Framing.GetTileSafely(i, j).TileType == x)))
                        total++;

                    if (total > 40)
                        goto reset;
                }
            }

            VerdantArea = new Rectangle(center.X, center.Y, 1, 1);
            break;
        }

        GenerateCircles();
        p.Value = 0.25f;
        CleanForCaves();
        p.Value = 0.5f;
        AddStones();

        for (int i = VerdantArea.Left; i < VerdantArea.Right; ++i) //Smooth out the biome!
            for (int j = VerdantArea.Top; j < VerdantArea.Bottom; ++j)
                if (WorldGen.genRand.Next(7) <= 3)
                    Tile.SmoothSlope(i, j, false);

        p.Message = "Growing vines...";
        p.Value = 0.6f;
        Vines();
        p.Message = "Growing flowers...";
        p.Value = 0.7f;
        AddPlants();
        p.Message = "Watering plants...";
        p.Value = 0.8f;
        AddWater();
        AddWaterfalls();
        p.Message = "Growing surface...";
        p.Value = 0.9f;
        AddSurfaceTree();
    }

    private static void AddWaterfalls()
    {
        for (int i = 0; i < 50 * WorldSize; ++i)
        {
            int x = WorldGen.genRand.Next(VerdantArea.Left, VerdantArea.Right);
            int y = WorldGen.genRand.Next(VerdantArea.Top, VerdantArea.Bottom);
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

    public static void AddSurfaceTree()
    {
        int offset = 0;
    retry:
        int x = (VerdantArea.Center.X + (WorldGen.genRand.NextBool() ? -offset : offset));
        int top = Helper.FindDown(new Vector2(x, 200) * 16);
        Point16 size = Point16.Zero;

        if (!StructureHelper.Generator.GetDimensions("World/Structures/SurfaceTree", VerdantMod.Instance, ref size))
            return;

        if (top <= Main.worldSurface * 0.36f)
        {
            offset += offset + WorldGen.genRand.Next(10, 20);
            goto retry;
        }

        Point16 spawnPos = new(x, top - size.Y + 12);

        if (Helper.TileRectangle(spawnPos.X, spawnPos.Y, size.X, size.Y, TileID.Cloud, TileID.RainCloud, TileID.LeafBlock) > 0)
        {
            offset += offset + WorldGen.genRand.Next(10, 20);
            goto retry;
        }

        int tryRepeats = 0;
        while (tryRepeats < 20 && Helper.AnyTileRectangle(spawnPos.X - 6, spawnPos.Y + 20 + tryRepeats, size.X, size.Y + 36) < 60)
            tryRepeats++;
        
        if (tryRepeats >= 20)
        {
            offset += offset + WorldGen.genRand.Next(10, 20);
            goto retry;
        }

        if (Helper.AnyTileRectangle(spawnPos.X - 6, spawnPos.Y, size.X, 20) > size.X * 20 * 0.8f)
        {
            offset += offset + WorldGen.genRand.Next(10, 20);
            goto retry;
        }

        StructureHelper.Generator.GenerateStructure("World/Structures/SurfaceTree", spawnPos + new Point16(0, tryRepeats), VerdantMod.Instance);
    }

    private static void AddStones()
    {
        for (int i = 0; i < 50 * WorldSize; ++i) //Stones
        {
            Point p = new(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
            while (!TileHelper.ActiveType(p.X, p.Y, ModContent.TileType<LushSoil>()))
                p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
            WorldGen.TileRunner(p.X, p.Y, WorldGen.genRand.NextFloat(7, 15), WorldGen.genRand.Next(5, 15), TileID.Stone, false, 0, 0, false, true);
        }

        for (int i = 0; i < 12 * WorldSize; ++i) //Ores
        {
            Point p = new(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
            while (!TileHelper.ActiveType(p.X, p.Y, ModContent.TileType<LushSoil>()))
                p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
            WorldGen.TileRunner(p.X, p.Y, WorldGen.genRand.NextFloat(2, 8), WorldGen.genRand.Next(5, 15), TileID.Gold, false, 0, 0, false, true);

            p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
            while (!TileHelper.ActiveType(p.X, p.Y, ModContent.TileType<LushSoil>()))
                p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
            WorldGen.TileRunner(p.X, p.Y, WorldGen.genRand.NextFloat(2, 8), WorldGen.genRand.Next(5, 15), TileID.Platinum, false, 0, 0, false, true);
        }

        for (int i = 0; i < 12 * WorldSize; ++i) //Aquamarine
        {
            Point p = new(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
            while (!TileHelper.ActiveType(p.X, p.Y, ModContent.TileType<LushSoil>()))
                p = new Point(WorldGen.genRand.Next(VerdantArea.X, VerdantArea.Right), WorldGen.genRand.Next(VerdantArea.Y, VerdantArea.Bottom));
            WorldGen.TileRunner(p.X, p.Y, WorldGen.genRand.NextFloat(2, 4), WorldGen.genRand.Next(5, 15), ModContent.TileType<EmbeddedAquamarine>(), false, 0, 0, false, true);
        }
    }

    private static void AddWater()
    {
        for (int i = 0; i < 30 * WorldSize; ++i)
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

    private void GenerateCircles()
    {
        VerdantCircles.Clear();
        float repeats = 6 * WorldSize;

        if (!WorldGen.remixWorldGen)
        {
            VerdantArea = new Rectangle(VerdantArea.Center.X - (int)(3f * WorldSize * WorldGen.genRand.Next(75, 85)) - 20, VerdantArea.Center.Y - 100, (int)(7 * WorldSize * WorldGen.genRand.Next(75, 85)), 200);
            VerdantArea.Location = new Point(VerdantArea.Location.X - 40, VerdantArea.Location.Y - 40);
            VerdantArea.Width += 80;
            VerdantArea.Height += 80;
        }
        else // dontdigup gen
        {
            VerdantArea = new Rectangle(VerdantArea.Center.X - 20, VerdantArea.Center.Y, 200, (int)(5 * WorldSize * WorldGen.genRand.Next(75, 85)));
            VerdantArea.Width += 40;
            VerdantArea.Height += 40;
        }

        for (int i = 0; i < repeats; ++i)
        {
            int x;
            int y;

            if (!WorldGen.remixWorldGen) 
            {
                x = (int)MathHelper.Lerp(VerdantArea.X + 50, VerdantArea.Right - 50, i / repeats);
                y = VerdantArea.Center.Y - WorldGen.genRand.Next(-20, 20);
            }
            else
            {
                x = VerdantArea.Center.X - WorldGen.genRand.Next(-20, 20);
                y = (int)MathHelper.Lerp(VerdantArea.Y + 50, VerdantArea.Bottom - 50, i / repeats);
            }

            VerdantCircles.Add(new GenCircle((int)(WorldGen.genRand.Next(50, 80) * WorldSize), new Point16(x, y)));
        }

        for (int i = 0; i < VerdantCircles.Count; ++i)
            VerdantCircles[i].FindTiles();
    }

    private void CleanForCaves()
    {
        const float Buffer = 3f;

        //Caves
        VerdantSystem.genNoise = WorldGen.remixWorldGen ? new FastNoise(WorldGen._genRandSeed)
        {
            Seed = WorldGen._genRandSeed,
            Frequency = 0.05f,
            NoiseType = FastNoise.NoiseTypes.CubicFractal, //Sets noise to proper type
            FractalType = FastNoise.FractalTypes.Billow
        } 
        : new FastNoise(WorldGen._genRandSeed)
        {
            Seed = WorldGen._genRandSeed,
            Frequency = 0.025f,
            NoiseType = FastNoise.NoiseTypes.PerlinFractal,
            FractalType = FastNoise.FractalTypes.Billow
        };

        int startX = VerdantArea.Center.X - (int)(Main.maxTilesX / Buffer);
        int endX = VerdantArea.Center.X + (int)(Main.maxTilesX / Buffer);

        int startY = VerdantArea.Center.Y - (int)(Main.maxTilesY / (Buffer * 2));
        int endY = VerdantArea.Center.Y + (int)(Main.maxTilesY / (Buffer * 2));

        HashSet<Point16> aggregateTiles = GenCircle.Locations;
        GetVerdantArea(aggregateTiles);

        Mod.Logger.Info("Placing base");

        foreach (var point in aggregateTiles)
        {
            Tile t = Framing.GetTileSafely(point.X, point.Y);
            float n = VerdantSystem.genNoise.GetNoise(point.X, point.Y);

            t.ClearEverything();

            if (n < -0.85f)
                continue;

            if (n < -0.67f) 
            { }
            else if (n < -0.57f)
                WorldGen.PlaceTile(point.X, point.Y, TileTypes[0], true);
            else
                WorldGen.PlaceTile(point.X, point.Y, TileTypes[1], true);

            if (n < -0.85f)
                WorldGen.KillWall(point.X, point.Y, false);
            else if (n < -0.52f)
                WorldGen.PlaceWall(point.X, point.Y, WallTypes[0], true);
            else
                WorldGen.PlaceWall(point.X, point.Y, WallTypes[1], true);
        }

        VerdantSystem.genNoise.Seed = WorldGen._genRandSeed;
        VerdantSystem.genNoise.Frequency = WorldGen.remixWorldGen ? 0.03f : 0.014f;
        VerdantSystem.genNoise.NoiseType = WorldGen.remixWorldGen ? FastNoise.NoiseTypes.PerlinFractal : FastNoise.NoiseTypes.ValueFractal;
        VerdantSystem.genNoise.FractalType = FastNoise.FractalTypes.Billow;
        VerdantSystem.genNoise.InterpolationMethod = FastNoise.Interp.Quintic;

        Mod.Logger.Info("Placing roots");

        foreach (var point in aggregateTiles)
        {
            float n = VerdantSystem.genNoise.GetNoise(point.X, point.Y);

            if (n > -0.4f)
                continue;

            Tile t = Framing.GetTileSafely(point.X, point.Y);

            if (t.WallType == WallTypes[0] && n < -0.4f)
                GenHelper.ReplaceWall(point, WorldGen.remixWorldGen ? WallTypes[1] : WallTypes[2]);

            if (n < -0.72f && TileTypes.Any(x => x == t.TileType) && t.TileType != TileTypes[0] && t.HasTile)
                GenHelper.ReplaceTile(point, TileTypes[4]);
        }

        GenCircle.Locations.Clear();
    }

    private static void GetVerdantArea(HashSet<Point16> aggregateTiles)
    {
        int left = Main.maxTilesX;
        int right = 0;
        int top = Main.maxTilesY;
        int bottom = 0;

        foreach (var point in aggregateTiles)
        {
            if (point.X < left)
                left = point.X;

            if (point.X > right)
                right = point.X;

            if (point.Y < top)
                top = point.Y;

            if (point.Y > bottom)
                bottom = point.Y;
        }

        VerdantArea = new Rectangle(left, top, right - left, bottom - top);
    }
}