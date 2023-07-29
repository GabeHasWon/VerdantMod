using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks;

public class LushSoilBlock : ModItem
{
    public override void SetStaticDefaults() => ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<PestControl.ThornBlock>();
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<LushSoil>());
}
