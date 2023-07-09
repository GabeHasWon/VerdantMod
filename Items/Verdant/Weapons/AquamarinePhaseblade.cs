using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Aquamarine;

namespace Verdant.Items.Verdant.Weapons;

class AquamarinePhaseblade : ModItem
{
    public override void SetDefaults() => Item.CloneDefaults(ItemID.WhitePhaseblade);

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupSystem.AquamarineRecipeGroup, 15)
            .AddIngredient(ItemID.MeteoriteBar, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}
