using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Blocks;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.Marble
{
    public class ThePlantItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "The Plant", "It feels powerful...");
        public override void SetDefaults()
        {
            QuickItem.SetBlock(this, 32, 30, TileType<LushSoil>());
            item.rare = ItemRarityID.Expert;
        }
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemID.MarbleBlock, 4), (ItemType<LushLeaf>(), 8));
    }
}
