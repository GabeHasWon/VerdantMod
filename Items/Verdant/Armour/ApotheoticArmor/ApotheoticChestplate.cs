using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Armour.ApotheoticArmor;

[AutoloadEquip(EquipType.Body)]
public class ApotheoticChestplate : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 36;
        Item.height = 20;
        Item.value = Item.buyPrice(0, 5, 0, 0);
        Item.rare = ItemRarityID.Blue;
        Item.defense = 20;
    }

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Summon) += 0.1f;
        player.maxMinions++;
        player.wingTimeMax = (int)(player.wingTimeMax * 1.2f);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<MysteriaWood>(20)
            .AddIngredient<YellowBulb>(2)
            .AddIngredient(ItemID.ChlorophyteBar, 15)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}