using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Plants;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks
{
    public class VerdantStrongVineMaterial : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Hardy Vine", "'It takes quite the sharp blade to cut through these'");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, TileType<VerdantStrongVine>());
    }
}
