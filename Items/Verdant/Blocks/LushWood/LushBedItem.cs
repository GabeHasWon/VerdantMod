using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class LushBedItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Bed", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, TileType<Tiles.Verdant.Decor.LushFurniture.LushBed>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemType<VerdantWoodBlock>(), 15), (ItemID.Silk, 5));
    }
}