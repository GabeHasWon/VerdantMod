using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    [Sacrifice(1)]
    public class WisplantCandelabraItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Wisplant Candelabra", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 30, 16, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.WisplantCandelabra>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 6), (ModContent.ItemType<Lightbulb>(), 1), (ModContent.ItemType<WisplantItem>(), 2));
    }
}