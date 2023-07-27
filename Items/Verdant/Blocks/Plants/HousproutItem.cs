using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks.Plants;

public class HousproutItem : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) => false;
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<Housprout>());
}
