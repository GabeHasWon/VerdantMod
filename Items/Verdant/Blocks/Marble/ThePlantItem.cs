using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Decor.Marble;

namespace Verdant.Items.Verdant.Blocks.Marble
{
    public class ThePlantItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "The Plant", "'in real'");

        public override void SetDefaults()
        {
            QuickItem.SetBlock(this, 32, 30, ModContent.TileType<ThePlant>());
            Item.rare = ItemRarityID.Expert;
        }

        public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, TileID.WorkBenches, 1, (ItemID.MarbleBlock, 4), (ModContent.ItemType<LushLeaf>(), 8));
    }
}
