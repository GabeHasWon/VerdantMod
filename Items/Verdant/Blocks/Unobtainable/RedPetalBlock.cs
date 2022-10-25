using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.Unobtainable
{
    public class RedPetalBlock : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Red Petal Block", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<VerdantRedPetal>());
    }
}
