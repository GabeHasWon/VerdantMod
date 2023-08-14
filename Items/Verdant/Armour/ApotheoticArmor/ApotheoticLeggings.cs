using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Armour.ApotheoticArmor;

[AutoloadEquip(EquipType.Legs)]
public class ApotheoticLeggings : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 20;
        Item.value = Item.buyPrice(0, 2, 0, 0);
        Item.rare = ItemRarityID.Blue;
        Item.defense = 12;
    }

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Summon) += 0.05f;
        player.maxMinions++;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<MysteriaWood>(8)
            .AddIngredient<ApotheoticSoul>(1)
            .AddIngredient(ItemID.ChlorophyteBar, 6)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}