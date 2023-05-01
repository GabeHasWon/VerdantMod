using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

[Sacrifice(1)]
public class MysteriaCandelabraItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Mysteria Candelabra", "");
    public override void SetDefaults() => QuickItem.SetBlock(this, 24, 26, ModContent.TileType<Tiles.Verdant.Decor.MysteriaFurniture.MysteriaCandelabra>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<MysteriaWood>(), 3), (ItemID.Torch, 1));
}