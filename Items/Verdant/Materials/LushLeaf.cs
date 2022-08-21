using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;

namespace Verdant.Items.Verdant.Materials
{
    class LushLeaf : ModItem
    {
        public override void SetDefaults() => QuickItem.SetMaterial(this, 12, 12, ItemRarityID.White);
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Leaf", "'Quite durable'");

        public override void AddRecipes()
        {
            QuickItem.AddRecipe(this, Mod, -1, 1, (ModContent.ItemType<VerdantLeafWallItem>(), 4));

            QuickItem.AddRecipe(ItemID.BrightGreenDye, Mod, TileID.DyeVat, 1, (ModContent.ItemType<LushLeaf>(), 8), (ItemID.SilverDye, 1));
            QuickItem.AddRecipe(ItemID.GreenandBlackDye, Mod, TileID.DyeVat, 1, (ModContent.ItemType<LushLeaf>(), 8), (ItemID.BlackDye, 1));
        }
    }
}
