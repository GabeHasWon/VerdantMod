using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Walls.Verdant;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.Walls
{
    public class LushWoodWallItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Wood Wall", "");
        public override void SetDefaults() => QuickItem.SetWall(this, 32, 32, WallType<LushWoodWall>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, -1, 4, (ItemType<LushWood.VerdantWoodBlock>(), 1));
    }
}