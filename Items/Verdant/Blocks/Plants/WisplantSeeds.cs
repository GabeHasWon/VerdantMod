using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Plants
{
    public class WisplantSeeds : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Wisplant Seeds");

        public override void SetDefaults()
        {
            QuickItem.SetBlock(this, 22, 28, ModContent.TileType<Tiles.Verdant.Basic.Plants.Wisplant>());
            Item.value = Item.buyPrice(0, 0, 0, 2);
        }
    }
}