using System;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks
{
    public class VerdantFlowerBulb : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Flower Bulb");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, TileType<Tiles.Verdant.Mounted.Flower_2x2>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, -1, 1, (ItemType<PinkPetal>(), 4), (ItemType<RedPetal>(), 4));
    }
}