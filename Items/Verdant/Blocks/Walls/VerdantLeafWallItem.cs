using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Walls.Verdant;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.Walls
{
    public class VerdantLeafWallItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Leaf Wall", "");
        public override void SetDefaults() => QuickItem.SetWall(this, 32, 32, WallType<VerdantLeafWall>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, -1, 4, (ItemType<LushLeaf>(), 1));
    }
}