using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    [Sacrifice(1)]
    public class LushWorkbenchItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Workbench", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 30, 16, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushWorkbench>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, -1, 1, (ModContent.ItemType<VerdantWoodBlock>(), 10));
    }
}