using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    [Sacrifice(1)]
    public class LushLanternItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Lantern", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushLantern>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<VerdantWoodBlock>(), 6), (ItemID.Torch, 1));
    }
}