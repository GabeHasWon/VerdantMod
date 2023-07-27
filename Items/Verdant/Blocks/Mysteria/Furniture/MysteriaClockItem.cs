using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

[Sacrifice(1)]
public class MysteriaClockItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 24, 48, ModContent.TileType<Tiles.Verdant.Decor.MysteriaFurniture.MysteriaClock>());

    public override void AddRecipes()
    {
        CreateRecipe(1)
            .AddIngredient(ModContent.ItemType<MysteriaWood>(), 10)
            .AddRecipeGroup(RecipeGroupID.IronBar, 3)
            .AddIngredient(ItemID.Glass, 6)
            .Register();
    }
}