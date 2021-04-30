using System;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class LushWoodCandleItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Candle", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 40, 28, TileType<Tiles.Verdant.Decor.LushFurniture.LushCandle>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemType<VerdantWoodBlock>(), 4), (ItemID.Torch, 1));
    }
}
