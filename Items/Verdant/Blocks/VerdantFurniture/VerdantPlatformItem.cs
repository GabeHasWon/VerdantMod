using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    [Sacrifice(1)]
    public class VerdantPlatformItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 10, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantPlatforms>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 2, (ModContent.ItemType<LushLeaf>(), 1));
    }
}