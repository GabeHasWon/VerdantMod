using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class LushClockItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Clock", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 24, 48, TileType<Tiles.Verdant.Decor.LushFurniture.LushClock>());
        public override void AddRecipes()
        {
            ModRecipe r = new ModRecipe(mod);
            r.AddIngredient(ItemType<VerdantWoodBlock>(), 10);
            r.AddRecipeGroup("IronBar", 3);
            r.AddIngredient(ItemID.Glass, 6);
            r.SetResult(this, 1);
            r.AddRecipe();
        }
    }
}