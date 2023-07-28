using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Aquamarine;

namespace Verdant.Items.Verdant.Blocks.Aquamarine;

public class EmbeddedAquamarineItem : ModItem
{
    public override void SetStaticDefaults() => ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true;
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<EmbeddedAquamarine>());

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<LushSoilBlock>())
            .AddRecipeGroup(RecipeGroupSystem.AquamarineRecipeGroup)
            .AddTile(TileID.HeavyWorkBench)
            .AddCondition(Condition.InGraveyard)
            .Register();
    }
}

public class EmbeddedStoneAquamarineItem : ModItem
{
    public override void SetStaticDefaults() => ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true;
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<EmbeddedStoneAquamarine>());

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.StoneBlock)
            .AddRecipeGroup(RecipeGroupSystem.AquamarineRecipeGroup)
            .AddTile(TileID.HeavyWorkBench)
            .AddCondition(Condition.InGraveyard)
            .Register();
    }
}
