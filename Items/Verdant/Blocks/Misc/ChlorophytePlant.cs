using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Trees;

namespace Verdant.Items.Verdant.Blocks.Misc;

public class ChlorophytePlant : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<ChlorophyteTree>());

    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient(ItemID.ChlorophyteOre, 5).
            AddIngredient(ModContent.ItemType<GreenCrystalItem>(), 1).
            AddCondition(RecipeConditions.AfterApotheosisDownedPlantera).
            Register();
    }
}
