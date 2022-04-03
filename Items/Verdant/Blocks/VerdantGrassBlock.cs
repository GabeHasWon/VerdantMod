using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks
{
    public class VerdantGrassBlock : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Grass Block", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<VerdantGrassLeaves>());
    }
}
