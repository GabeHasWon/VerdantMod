using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class LushPlatformItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Platform", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 10, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushPlatform>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, -1, 2, (ModContent.ItemType<VerdantWoodBlock>(), 1));
    }
}