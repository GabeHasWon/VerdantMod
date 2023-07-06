using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks.Plants;

public class WaterberryBushItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Waterberry Bush", "Can be placed on itself and Verdant leaves\nOnly grows in water");
    public override void SetDefaults() => QuickItem.SetBlock(this, 32, 20, ModContent.TileType<WaterberryBushPicked>());
}
