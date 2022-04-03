using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls
{
    public class VerdantPinkPetalWallItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Pink Petal Wall", "");
        public override void SetDefaults() => QuickItem.SetWall(this, 16, 16, ModContent.WallType<VerdantPinkPetalWall>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ModContent.ItemType<PinkPetal>(), 4));
    }
}