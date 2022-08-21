using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls
{
    public class VerdantRedPetalWallItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Red Petal Wall", "");
        public override void SetDefaults() => QuickItem.SetWall(this, 16, 16, ModContent.WallType<VerdantRedPetalWall>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, TileID.WorkBenches, 1, (ModContent.ItemType<RedPetal>(), 4));
    }
}
