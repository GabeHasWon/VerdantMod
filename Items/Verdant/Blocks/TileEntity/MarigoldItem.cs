using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks.TileEntity
{
    public class MarigoldItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Marigold", "'Smells...metallic?'");
        public override void SetDefaults() => QuickItem.SetBlock(this, 28, 28, ModContent.TileType<MarigoldTile>());
    }
}
