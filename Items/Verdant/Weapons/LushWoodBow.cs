using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;

namespace Verdant.Items.Verdant.Weapons;

class LushWoodBow : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.WoodenBow);
        Item.damage += 2;
        Item.useTime -= 2;
        Item.useAnimation -= 2;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<VerdantWoodBlock>(), 10)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
