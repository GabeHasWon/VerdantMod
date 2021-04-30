using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class VerdantWoodChestBlock : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Wood Chest", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, TileType<Tiles.Verdant.Decor.LushFurniture.VerdantWoodChest>());
    }
}
