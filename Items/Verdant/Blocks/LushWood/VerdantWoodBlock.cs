using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class VerdantWoodBlock : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Wood", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 32, 18, ModContent.TileType<VerdantWood>());
        public override void AddRecipes()
        {
            QuickItem.AddRecipe(this, mod, -1, 1, (ModContent.ItemType<Walls.LushWoodWallItem>(), 4));
            QuickItem.AddRecipe(this, mod, -1, 1, (ModContent.ItemType<LushPlatformItem>(), 2));
        }
    }
}