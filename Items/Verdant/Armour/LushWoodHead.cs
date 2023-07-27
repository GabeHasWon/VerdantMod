using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;

namespace Verdant.Items.Verdant.Armour;

[AutoloadEquip(EquipType.Head)]
public class LushWoodHead : ModItem
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Lush Wood Helmet");
        // Tooltip.SetDefault("+1 flat summon damage");
    }

    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 20;
        Item.value = 10000;
        Item.rare = ItemRarityID.Blue;
        Item.defense = 2;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<LushWoodBody>() && legs.type == ModContent.ItemType<LushWoodLegs>();
    public override void UpdateEquip(Player player) => player.GetDamage(DamageClass.Summon).Flat += 1;

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = "+1 flat summon damage";
        player.GetDamage(DamageClass.Summon).Flat += 1;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<VerdantWoodBlock>(12)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}