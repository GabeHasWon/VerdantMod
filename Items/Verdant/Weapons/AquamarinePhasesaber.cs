using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Weapons;

class AquamarinePhasesaber : ModItem
{
    public override void SetDefaults() => Item.CloneDefaults(ItemID.WhitePhasesaber);

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<AquamarinePhaseblade>()
            .AddIngredient(ItemID.CrystalShard, 25)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}
