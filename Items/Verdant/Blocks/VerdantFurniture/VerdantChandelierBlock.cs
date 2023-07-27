using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    [Sacrifice(1)]
    public class VerdantChandelierBlock : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 32, 42, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantChandelier>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 6), (ModContent.ItemType<RedPetal>(), 6), (ModContent.ItemType<VerdantStrongVineMaterial>(), 4), (ModContent.ItemType<Lightbulb>(), 3));
    }
}