using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantChandelierLargeItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Chandelier (Large)", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantChandelierLarge>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemType<LushLeaf>(), 10), (ItemType<PinkPetal>(), 6), (ItemType<VerdantStrongVineMaterial>(), 2), (ItemType<Lightbulb>(), 3));
    }
}