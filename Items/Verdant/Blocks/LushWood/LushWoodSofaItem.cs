using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    [Sacrifice(1)]
    public class LushWoodSofaItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Sofa", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 48, 28, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushSofa>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.Sawmill, 1, (ModContent.ItemType<VerdantWoodBlock>(), 5), (ItemID.Silk, 1));
    }
}