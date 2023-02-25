using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    [Sacrifice(1)]
    public class LushTableItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Table", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 40, 28, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushTable>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, Terraria.ID.TileID.WorkBenches, 1, (ModContent.ItemType<VerdantWoodBlock>(), 8));
    }
}
