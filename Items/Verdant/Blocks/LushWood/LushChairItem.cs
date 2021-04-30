using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class LushChairItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Chair", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, TileType<Tiles.Verdant.Decor.LushFurniture.LushChair>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, Terraria.ID.TileID.WorkBenches, 1, (ItemType<VerdantWoodBlock>(), 4));
    }
}