using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

[Sacrifice(1)]
public class MysteriaTableItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 40, 28, ModContent.TileType<Tiles.Verdant.Decor.MysteriaFurniture.MysteriaTable>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, Terraria.ID.TileID.WorkBenches, 1, (ModContent.ItemType<MysteriaWood>(), 8));
}
