using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Food;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks.Plants;

public class TearBulbItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 28, 26, ModContent.TileType<TearBulb>(), rarity: ItemRarityID.Blue);
    public override void AddRecipes() => CreateRecipe().AddIngredient<LushLeaf>(10).AddIngredient<Waterberry>().AddCondition(Condition.InGraveyard).Register();
}
