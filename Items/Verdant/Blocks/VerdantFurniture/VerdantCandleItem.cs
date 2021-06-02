using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantCandleItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Candle", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantCandle>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, Terraria.ID.TileID.WorkBenches, 1, (ItemType<VerdantStrongVineMaterial>(), 1), (ItemType<Lightbulb>(), 1));
    }
}