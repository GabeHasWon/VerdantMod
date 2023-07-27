using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.Misc;

public class SnailShellBlockItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<SnailShellBlock>());
}
