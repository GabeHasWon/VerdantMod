using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    [Sacrifice(1)]
    public class VerdantWorkbenchItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 30, 16, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantWorkbench>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<LushLeaf>(), 6), (ModContent.ItemType<RedPetal>(), 4));
    }
}