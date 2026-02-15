using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks.Plants;

public class SmokeBulbItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 28, 26, ModContent.TileType<SmokeBulb>(), rarity: ItemRarityID.Blue);
    public override void AddRecipes() => CreateRecipe().AddIngredient<LushLeaf>(15).AddCondition(Condition.InGraveyard).Register();
}
