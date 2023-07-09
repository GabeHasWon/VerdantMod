using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Decor.AquamarineDecor;

namespace Verdant.Items.Verdant.Blocks.Aquamarine;

public class AquamarineTorchItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Aquamarine Torch");
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 16, ModContent.TileType<AquamarineTorch>());

    public override void AddRecipes()
    {
        CreateRecipe(10)
            .AddRecipeGroup(RecipeGroupSystem.AquamarineRecipeGroup)
            .AddIngredient(ItemID.Torch, 10)
            .Register();
    }
}
