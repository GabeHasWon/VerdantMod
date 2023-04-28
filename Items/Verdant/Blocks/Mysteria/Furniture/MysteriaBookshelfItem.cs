using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

[Sacrifice(1)]
public class MysteriaBookshelfItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Mysteria Bookcase", "");
    public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, ModContent.TileType<Tiles.Verdant.Decor.MysteriaFurniture.MysteriaBookshelf>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<MysteriaWood>(), 20), (ItemID.Book, 10));
}