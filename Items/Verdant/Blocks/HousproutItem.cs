using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Plants;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks
{
    public class HousproutItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Housprout", "'A magnificent growth said to build a small house almost instantly'");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, TileType<Housprout>());
    }
}
