using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    [Sacrifice(1)]
    public class LushDresserItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 50, 30, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushDresser>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<VerdantWoodBlock>(), 16));
    }
}