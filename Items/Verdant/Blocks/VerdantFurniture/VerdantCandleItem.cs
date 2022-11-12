using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    [Sacrifice(1)]
    public class VerdantCandleItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Candle", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantCandle>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, Terraria.ID.TileID.LivingLoom, 1, (ModContent.ItemType<VerdantStrongVineMaterial>(), 1), (ModContent.ItemType<Lightbulb>(), 1));
    }
}