using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    [Sacrifice(1)]
    public class LushCandleItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Candle", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushCandle>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, TileID.WorkBenches, 1, (ModContent.ItemType<VerdantWoodBlock>(), 2), (ItemID.Torch, 1));
    }
}