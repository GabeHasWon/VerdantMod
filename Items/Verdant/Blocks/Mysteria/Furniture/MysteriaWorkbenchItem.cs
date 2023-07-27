using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

[Sacrifice(1)]
public class MysteriaWorkbenchItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 30, 16, ModContent.TileType<Tiles.Verdant.Decor.MysteriaFurniture.MysteriaWorkbench>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<MysteriaWood>(), 10));
}