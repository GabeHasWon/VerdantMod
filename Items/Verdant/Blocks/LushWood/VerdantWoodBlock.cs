using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class VerdantWoodBlock : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Wood", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 32, 18, TileType<VerdantWood>());
        public override void AddRecipes()
        {
            QuickItem.AddRecipe(this, mod, -1, 1, (ItemType<Walls.LushWoodWallItem>(), 4));
            QuickItem.AddRecipe(this, mod, -1, 1, (ItemType<LushPlatformItem>(), 2));
        }
    }
}