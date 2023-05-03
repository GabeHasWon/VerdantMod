using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.Misc;

public class SnailShellBlockItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Snail Shell", "'It feels bad to have these'");
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<SnailShellBlock>());
}
