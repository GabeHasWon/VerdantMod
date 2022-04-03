using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantLanternItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Lantern", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantLantern>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 6), (ModContent.ItemType<Lightbulb>(), 1));
    }
}