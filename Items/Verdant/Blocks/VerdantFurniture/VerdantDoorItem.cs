using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantDoorItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Door");
        public override void SetDefaults() => QuickItem.SetBlock(this, 42, 26, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantDoorClosed>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.LivingLoom, 1, (ModContent.ItemType<RedPetal>(), 6), (ModContent.ItemType<LushLeaf>(), 12));
    }
}