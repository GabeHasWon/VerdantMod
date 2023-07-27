using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Mounted.Furniture;

namespace Verdant.Items.Verdant.Blocks.Misc;

internal class ApotheoticPaintingItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<ApotheoticPainting>());
}

internal class LightbulbPaintingItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<LightbulbPainting>());
}