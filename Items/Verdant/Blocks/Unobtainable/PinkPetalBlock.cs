using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.Unobtainable
{
    public class PinkPetalBlock : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<VerdantPinkPetal>(), maxStack: 99999);
    }
}
