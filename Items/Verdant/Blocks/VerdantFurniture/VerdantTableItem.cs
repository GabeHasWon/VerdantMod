using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantTableItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Table", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 40, 28, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantTable>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, Terraria.ID.TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 6), (ModContent.ItemType<VerdantStrongVineMaterial>(), 3), (ModContent.ItemType<PinkPetal>(), 6));
    }
}