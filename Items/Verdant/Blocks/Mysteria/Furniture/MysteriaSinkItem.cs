using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

[Sacrifice(1)]
public class MysteriaSinkItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Mysteria Sink", "");
    public override void SetDefaults() => QuickItem.SetBlock(this, 32, 34, ModContent.TileType<Tiles.Verdant.Decor.MysteriaFurniture.MysteriaSink>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<MysteriaWood>(), 6), (ItemID.WaterBucket, 1));
}