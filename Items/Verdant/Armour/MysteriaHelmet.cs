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
        DisplayName.SetDefault("Mysteria Veil");
        Tooltip.SetDefault("+4 flat minion damage");
    }

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 24;
        Item.value = 0;
        Item.rare = ItemRarityID.Green;
        Item.defense = 4;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<MysteriaChest>() && legs.type == ModContent.ItemType<MysteriaLeggings>();
    public override void UpdateEquip(Player player) => player.GetDamage(DamageClass.Summon).Flat += 4;

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = "3 additional damage flat";
        player.GetDamage(DamageClass.Summon).Flat += 3;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<MysteriaClump>(), 12)
            .AddIngredient(ModContent.ItemType<MysteriaWood>(), 20)
            .AddTile(TileID.Anvils)
            .Register();
    }
}