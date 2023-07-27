using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Aquamarine;
using Verdant.Projectiles.Misc;

namespace Verdant.Items.Verdant.Tools;

[Sacrifice(1)]
internal class AquamarineHook : ModItem
{
    // public override void SetStaticDefaults() => DisplayName.SetDefault("Aquamarine Hook");

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.AmethystHook);
        Item.shootSpeed = 12.25f;
        Item.shoot = ModContent.ProjectileType<AquamarineHookProjectile>();
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupSystem.AquamarineRecipeGroup, 15)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}