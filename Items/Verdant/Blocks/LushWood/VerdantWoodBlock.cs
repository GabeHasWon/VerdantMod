using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    [Sacrifice(100)]
    public class VerdantWoodBlock : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 24, 20, ModContent.TileType<VerdantWood>());

        public override void AddRecipes()
        {
            QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<Walls.LushWoodWallItem>(), 4));
            QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<LushPlatformItem>(), 2));
        }
    }
}