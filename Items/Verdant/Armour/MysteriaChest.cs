using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Armour;

[AutoloadEquip(EquipType.Body)]
public class MysteriaChest : ModItem
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Mysteria Chestpiece");
        // Tooltip.SetDefault("Increased mining speed\nIncreased tile and wall placement speed");
    }

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 22;
        Item.value = Item.buyPrice(0, 0, 50, 0);
        Item.rare = ItemRarityID.Orange;
        Item.defense = 7;
    }

    public override void UpdateEquip(Player player)
    {
        player.pickSpeed -= 0.2f;
        player.tileSpeed += 0.2f;
        player.wallSpeed += 0.2f;
        player.blockRange++;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<MysteriaClump>(16)
            .AddIngredient<MysteriaWood>(20)
            .AddTile(TileID.Anvils)
            .Register();
    }
}