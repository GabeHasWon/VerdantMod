using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    [Sacrifice(1)]
    public class LushClockItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 24, 48, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushClock>());

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe(1);
            r.AddIngredient(ModContent.ItemType<VerdantWoodBlock>(), 10);
            r.AddRecipeGroup("IronBar", 3);
            r.AddIngredient(ItemID.Glass, 6);
            r.Register();
        }
    }
}