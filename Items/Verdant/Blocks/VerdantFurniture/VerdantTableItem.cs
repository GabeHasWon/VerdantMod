using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    [Sacrifice(1)]
    public class VerdantTableItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 40, 28, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantTable>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, Terraria.ID.TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 6), (ModContent.ItemType<VerdantStrongVineMaterial>(), 3), (ModContent.ItemType<PinkPetal>(), 6));
    }
}