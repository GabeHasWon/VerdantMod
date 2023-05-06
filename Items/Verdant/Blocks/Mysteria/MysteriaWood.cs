using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Trees;

namespace Verdant.Items.Verdant.Blocks.Mysteria;

public class MysteriaWood : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Mysteria Wood", "These blocks cannot be sloped");
    public override void SetDefaults() => QuickItem.SetBlock(this, 26, 18, ModContent.TileType<MysteriaTree>());
}
