using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantCandelabraItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Candelabra", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantCandelabra>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 3), (ModContent.ItemType<PinkPetal>(), 2), (ModContent.ItemType<Lightbulb>(), 1));
    }
}