using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.Misc;

public class OvergrownBrickItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Overgrown Gray Brick");
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<OvergrownBricks>());
}
