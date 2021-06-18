using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class LushBathtubItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Bathtub", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, TileType<Tiles.Verdant.Decor.LushFurniture.LushBathtub>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemType<VerdantWoodBlock>(), 14));
    }
}