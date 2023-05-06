using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Armour;

[AutoloadEquip(EquipType.Body)]
public class MysteriaChest : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Mysteria Chestpiece");
        Tooltip.SetDefault("Increased mining speed\nIncreased tile and wall placement speed");
    }

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 22;
        Item.value = 10000;
        Item.rare = ItemRarityID.Green;
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
        Recipe m = CreateRecipe();
        m.AddIngredient(ModContent.ItemType<VerdantStrongVineMaterial>(), 6);
        m.AddIngredient(ModContent.ItemType<PinkPetal>(), 14);
        m.AddIngredient(ModContent.ItemType<LushLeaf>(), 20);
        m.AddIngredient(ModContent.ItemType<YellowBulb>(), 2);
        m.AddTile(TileID.Anvils);
        m.Register();
    }
}