using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Decor.Marble;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.Marble
{
    public class ThePlantItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "The Plant", "'in real'");

        public override void SetDefaults()
        {
            QuickItem.SetBlock(this, 32, 30, TileType<ThePlant>());
            item.rare = ItemRarityID.Expert;
        }

        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemID.MarbleBlock, 4), (ItemType<LushLeaf>(), 8));
    }
}
