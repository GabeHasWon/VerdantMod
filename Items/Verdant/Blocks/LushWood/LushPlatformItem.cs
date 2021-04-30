using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class LushPlatformItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Platform", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 10, TileType<Tiles.Verdant.Decor.LushFurniture.LushPlatform>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, Terraria.ID.TileID.WorkBenches, 2, (ItemType<VerdantWoodBlock>(), 1));
    }
}