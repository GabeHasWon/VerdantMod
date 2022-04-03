using Terraria.ModLoader;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls
{
    public class LushWoodWallItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Wood Wall", "");
        public override void SetDefaults() => QuickItem.SetWall(this, 32, 32, ModContent.WallType<LushWoodWall>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, -1, 4, (ModContent.ItemType<LushWood.VerdantWoodBlock>(), 1));
    }
}