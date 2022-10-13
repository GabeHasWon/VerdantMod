using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;

namespace Verdant.Items.Verdant.Weapons;

class LushWoodHammer : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.WoodenHammer);
        Item.damage += 1;
        Item.useTime -= 2;
        Item.useAnimation -= 2;
        Item.hammer += 3;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<VerdantWoodBlock>(), 8)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
