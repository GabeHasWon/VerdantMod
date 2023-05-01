using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Armour;

[AutoloadEquip(EquipType.Legs)]
public class MysteriaLeggings : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Mysteria Leggings");
        Tooltip.SetDefault("+6 flat minion damage");
    }

    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 12;
        Item.value = 10000;
        Item.rare = ItemRarityID.Blue;
        Item.defense = 3;
    }

    public override void UpdateEquip(Player player) => player.GetDamage(DamageClass.Summon).Flat += 6;

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<MysteriaClump>(2)
            .AddIngredient<MysteriaWood>(14)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}