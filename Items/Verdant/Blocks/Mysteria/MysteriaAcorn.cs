using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Mysteria;

namespace Verdant.Items.Verdant.Blocks.Mysteria;

public class MysteriaAcorn : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 24, ModContent.TileType<MysteriaSprout>());
}
