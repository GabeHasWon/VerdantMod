using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class LushCandleItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Candle", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, TileType<Tiles.Verdant.Decor.LushFurniture.LushCandle>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemType<VerdantWoodBlock>(), 2), (ItemID.Torch, 1));
    }
}