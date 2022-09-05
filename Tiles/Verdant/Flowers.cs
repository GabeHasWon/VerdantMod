using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant;

public class Flowers
{
    public static Dictionary<int, FlowerInfo> FlowerIDs;

    public static void Load(Mod mod)
    {
        var types = mod.GetType().Assembly.GetTypes().Where(x => !x.IsAbstract && typeof(IFlowerTile).IsAssignableFrom(x));
        FlowerIDs = new();

        foreach (var type in types)
        {
            ModTile tile = mod.Find<ModTile>(type.Name);
            IFlowerTile flowerTile = tile as IFlowerTile;

            FlowerIDs.Add(tile.Type, new FlowerInfo(flowerTile.IsFlower, flowerTile.OffsetAt));
        }
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
