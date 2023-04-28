using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Armour;

[AutoloadEquip(EquipType.Head)]
public class MysteriaHelmet : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Mysteria Headdress");
        Tooltip.SetDefault("+6 flat minion damage");
    }

    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 20;
        Item.value = 0;
        Item.rare = ItemRarityID.Green;
        Item.defense = 5;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<VerdantChestplate>() && legs.type == ModContent.ItemType<VerdantLeggings>();

    public override void UpdateArmorSet(Player player) => player.GetDamage(DamageClass.Summon).Flat += 6;
    public override void UpdateEquip(Player player) => player.GetDamage(DamageClass.Summon).Flat += 6;

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<MysteriaClump>(), 12)
            .AddIngredient(ModContent.ItemType<MysteriaWood>(), 20)
            .AddTile(TileID.Anvils)
            .Register();
    }
}