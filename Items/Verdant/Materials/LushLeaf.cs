using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;

namespace Verdant.Items.Verdant.Materials
{
    class LushLeaf : ModItem
    {
        public override void SetDefaults() => QuickItem.SetMaterial(this, 12, 12, ItemRarityID.White);
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Leaf", "'Quite durable'");
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, -1, 1, (ModContent.ItemType<VerdantLeafWallItem>(), 4));
    }
}
