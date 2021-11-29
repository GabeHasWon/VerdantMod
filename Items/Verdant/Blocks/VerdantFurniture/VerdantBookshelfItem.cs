using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantBookshelfItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Bookcase", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 38, 52, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantBookshelf>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.LivingLoom, 1, (ItemType<LushLeaf>(), 14), (ItemType<RedPetal>(), 4), (ItemType<PinkPetal>(), 4), (ItemID.Book, 8));
    }
}