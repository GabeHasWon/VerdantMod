using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    [Sacrifice(1)]
    public class LushSinkItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 32, 34, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushSink>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<VerdantWoodBlock>(), 6), (ItemID.WaterBucket, 1));
    }
}