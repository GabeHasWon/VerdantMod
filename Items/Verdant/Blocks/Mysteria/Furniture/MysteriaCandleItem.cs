using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

[Sacrifice(1)]
public class MysteriaCandleItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<Tiles.Verdant.Decor.MysteriaFurniture.MysteriaCandle>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<MysteriaWood>(), 2), (ItemID.Torch, 1));
}