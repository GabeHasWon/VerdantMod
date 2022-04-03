using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantChandelierLargeItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Chandelier (Large)", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantChandelierLarge>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 10), (ModContent.ItemType<PinkPetal>(), 6), (ModContent.ItemType<VerdantStrongVineMaterial>(), 2), (ModContent.ItemType<Lightbulb>(), 3));
    }
}