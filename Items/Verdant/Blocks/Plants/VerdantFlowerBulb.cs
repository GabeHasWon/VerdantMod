using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.Plants
{
    public class VerdantFlowerBulb : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Flower Bulb");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<Tiles.Verdant.Mounted.Flower_2x2>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<PinkPetal>(), 4), (ModContent.ItemType<RedPetal>(), 4));
    }
}