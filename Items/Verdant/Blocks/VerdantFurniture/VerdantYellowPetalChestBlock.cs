using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Decor;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantYellowPetalChestBlock : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Yellow Petal Chest", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<VerdantYellowPetalChest>());
    }
}
