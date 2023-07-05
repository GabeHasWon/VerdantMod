using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Aquamarine;

namespace Verdant.Items.Verdant.Blocks.Aquamarine;

public class BackslateTileItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Backslate");
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<BackslateTile>());
}
