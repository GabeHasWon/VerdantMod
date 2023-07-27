using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Aquamarine;

namespace Verdant.Items.Verdant.Blocks.Aquamarine;

public class AquamarineItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 28, 28, ModContent.TileType<AquamarineTile>());
}
