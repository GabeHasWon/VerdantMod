using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Mounted.Furniture;

namespace Verdant.Items.Verdant.Blocks.Misc;

[Sacrifice(1)]
public class ApotheoticPaintingItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<ApotheoticPainting>());
}

[Sacrifice(1)]
public class LightbulbPaintingItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<LightbulbPainting>());
}