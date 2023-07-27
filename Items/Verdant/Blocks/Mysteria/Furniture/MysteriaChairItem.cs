using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

[Sacrifice(1)]
public class MysteriaChairItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<Tiles.Verdant.Decor.MysteriaFurniture.MysteriaChair>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, Terraria.ID.TileID.WorkBenches, 1, (ModContent.ItemType<MysteriaWood>(), 4));
}