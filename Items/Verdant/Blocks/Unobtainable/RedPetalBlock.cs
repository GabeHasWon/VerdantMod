using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.Unobtainable;

public class RedPetalBlock : ModItem
{
    public override void SetStaticDefaults() => ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true;
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<VerdantRedPetal>(), maxStack: 99999);
}
