using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantWorkbenchItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Workbench", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 30, 16, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantWorkbench>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, -1, 1, (ItemType<LushLeaf>(), 6), (ItemType<RedPetal>(), 4));
    }
}