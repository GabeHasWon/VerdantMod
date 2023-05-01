using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

[Sacrifice(1)]
public class MysteriaSofaItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Mysteria Sofa", "");
    public override void SetDefaults() => QuickItem.SetBlock(this, 48, 28, ModContent.TileType<Tiles.Verdant.Decor.MysteriaFurniture.MysteriaSofa>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.Sawmill, 1, (ModContent.ItemType<MysteriaWood>(), 5), (ItemID.Silk, 1));
}