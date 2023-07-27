using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    [Sacrifice(1)]
    public class LushPlatformItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 10, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushPlatform>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 2, (ModContent.ItemType<VerdantWoodBlock>(), 1));
    }
}