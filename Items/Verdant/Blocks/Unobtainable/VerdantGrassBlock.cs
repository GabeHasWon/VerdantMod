using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.Unobtainable;

public class VerdantGrassBlock : ModItem
{
    public override void SetStaticDefaults() => ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true;
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<VerdantGrassLeaves>(), maxStack: 99999);
}
