using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    [Sacrifice(1)]
    public class VerdantSofaItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 48, 32, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantSofa>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 10), (ModContent.ItemType<RedPetal>(), 4));
    }
}