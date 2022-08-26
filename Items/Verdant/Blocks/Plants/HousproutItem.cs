using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks.Plants
{
    public class HousproutItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Housprout", "WIP - WILL NOT BE FINISHED BEFORE RELEASE\n'A magnificent growth said to build a small house almost instantly'");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<Housprout>());
    }
}
