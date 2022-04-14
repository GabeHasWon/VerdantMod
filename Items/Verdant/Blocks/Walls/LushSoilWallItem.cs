using Terraria.ModLoader;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls
{
    public class LushSoilWallItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Soil Wall", "");
        public override void SetDefaults() => QuickItem.SetWall(this, 32, 32, ModContent.WallType<LushSoilWall>());
        public override void AddRecipes()
        {
            QuickItem.AddRecipe(this, mod, -1, 4, (ModContent.ItemType<LushSoilBlock>(), 1));
            QuickItem.AddRecipe(ModContent.ItemType<LushSoilBlock>(), mod, -1, 1, (item.type, 4));
        }
    }
}