using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Decor.Terrariums;

namespace Verdant.Items.Verdant.Blocks.Terrariums;

[Sacrifice(1)]
public class AquariumItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 48, 32, ModContent.TileType<Aquarium>());

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Terrarium, 1)
            .AddIngredient<LushLeaf>(4)
            .AddIngredient<LushSoilBlock>(2)
            .AddCondition(Condition.NearWater)
            .Register();
    }
}
