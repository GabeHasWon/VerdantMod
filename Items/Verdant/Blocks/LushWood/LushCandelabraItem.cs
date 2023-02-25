using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    [Sacrifice(1)]
    public class LushCandelabraItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Candelabra", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushCandelabra>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<VerdantWoodBlock>(), 3), (ItemID.Torch, 1));
    }
}