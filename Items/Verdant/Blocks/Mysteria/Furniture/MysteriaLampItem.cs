using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

[Sacrifice(1)]
public class MysteriaLampItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Mysteria Lamp", "");
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<Tiles.Verdant.Decor.MysteriaFurniture.MysteriaLamp>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<MysteriaWood>(), 6), (ItemID.Torch, 1));
}