using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls
{
    public class VerdantRedPetalWallItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetWall(this, 16, 16, ModContent.WallType<VerdantRedPetalWall>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 4, (ModContent.ItemType<RedPetal>(), 1));
    }
}
