using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;

namespace Verdant.Items.Verdant.Weapons;

class LushWoodSword : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.WoodenSword);
        Item.damage += 2;
        Item.useTime -= 2;
        Item.useAnimation -= 2;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<VerdantWoodBlock>(), 7)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
