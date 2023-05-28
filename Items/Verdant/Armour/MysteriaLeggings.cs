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
        Tooltip.SetDefault("Increased mining speed\nIncreased tile and wall placement speed");
    }

    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 12;
        Item.value = Item.buyPrice(0, 0, 50, 0);
        Item.rare = ItemRarityID.Orange;
        Item.defense = 3;
    }

    public override void UpdateEquip(Player player)
    {
        player.pickSpeed -= 0.1f;
        player.tileSpeed += 0.1f;
        player.wallSpeed += 0.1f;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<MysteriaClump>(2)
            .AddIngredient<MysteriaWood>(14)
            .AddTile(TileID.Anvils)
            .Register();
    }
}