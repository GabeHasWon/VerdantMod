using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class LushLanternItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Lantern", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, TileType<Tiles.Verdant.Decor.LushFurniture.LushLantern>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemType<VerdantWoodBlock>(), 6), (ItemID.Torch, 1));
    }
}