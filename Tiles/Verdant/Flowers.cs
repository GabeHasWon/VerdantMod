using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace Verdant.Tiles.Verdant;

public class Flowers
{
    public static Dictionary<int, FlowerInfo> FlowerIDs;

    public static void Load(Mod mod)
    {
        var types = AssemblyManager.GetLoadableTypes(mod.Code).Where(x => !x.IsAbstract && typeof(IFlowerTile).IsAssignableFrom(x));
        FlowerIDs = [];

        LoadVanillaFlowers();

        foreach (var type in types)
        {
            ModTile tile = mod.Find<ModTile>(type.Name);
            IFlowerTile flowerTile = tile as IFlowerTile;

            FlowerIDs.Add(tile.Type, new FlowerInfo(flowerTile.IsFlower, flowerTile.OffsetAt));
        }
    }

    private static void LoadVanillaFlowers()
    {
        FlowerIDs.Add(TileID.Plants, new FlowerInfo((i, j) => //Normal grass plants
        {
            int frameX = Main.tile[i, j].TileFrameX / 18;

            return frameX == 6 || frameX == 7 || frameX >= 9;
        }, (i, j) => new[] { new Vector2(8) }));

        FlowerIDs.Add(TileID.Sunflower, new FlowerInfo((i, j) => true, (i, j) => new[] { new Vector2(16, 8) })); //Sunflower

        FlowerIDs.Add(TileID.JunglePlants, new FlowerInfo((i, j) => //Jungle grass plants
        {
            int frameX = Main.tile[i, j].TileFrameX / 18;

            return frameX == 6 || frameX == 7 || frameX >= 9;
        }, (i, j) => new[] { new Vector2(8) }));

        FlowerIDs.Add(TileID.Plants2, new FlowerInfo((i, j) => (Main.tile[i, j].TileFrameX / 18) >= 6, (i, j) => new[] { new Vector2(8) })); //Tall normal plants
        FlowerIDs.Add(TileID.JunglePlants2, new FlowerInfo((i, j) => (Main.tile[i, j].TileFrameX / 18) >= 6, (i, j) => new[] { new Vector2(8) })); //Tall jungle plants
        FlowerIDs.Add(TileID.BloomingHerbs, new FlowerInfo((i, j) => true, (i, j) => new[] { new Vector2(8) })); //Blooming herbs

        FlowerIDs.Add(TileID.HallowedPlants, new FlowerInfo((i, j) => //Hallowed grass plants
        {
            int frameX = Main.tile[i, j].TileFrameX / 18;

            return frameX == 4 || frameX == 6 || frameX == 7 || frameX >= 10;
        }, (i, j) => new[] { new Vector2(8) }));
    }

    public struct FlowerInfo
    {
        public delegate bool CheckDelegate(int i, int j);
        public delegate Vector2[] OffsetsAtDelegate(int i, int j);

        public CheckDelegate IsValid;
        public OffsetsAtDelegate OffsetsAt;

        public FlowerInfo(CheckDelegate check, OffsetsAtDelegate index)
        {
            IsValid = check;
            OffsetsAt = index;
        }
    }
}
