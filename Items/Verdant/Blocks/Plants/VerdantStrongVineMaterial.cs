using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks.Plants;

public class VerdantStrongVineMaterial : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Hardy Vine", "Certain things can be placed on or under these\n" +
        "'It takes quite the sharp blade to cut through these'");
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<VerdantStrongVine>());
}
