using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Puff;

namespace Verdant.Items.Verdant.Blocks.Plants
{
    public class PinkPuff : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Pink Puff", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 30, 42, ModContent.TileType<BigPuff>());
    }
}
