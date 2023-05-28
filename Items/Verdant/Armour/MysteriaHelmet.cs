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
        Tooltip.SetDefault("Increased mining speed\nIncreased tile and wall placement speed\nIncreased placement range");
    }

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 24;
        Item.value = Item.buyPrice(0, 0, 50, 0);
        Item.rare = ItemRarityID.Orange;
        Item.defense = 5;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<MysteriaChest>() && legs.type == ModContent.ItemType<MysteriaLeggings>();

    public override void UpdateEquip(Player player)
    {
        player.pickSpeed -= 0.1f;
        player.tileSpeed += 0.1f;
        player.wallSpeed += 0.1f;
        player.blockRange++;
    }

    public override void UpdateArmorSet(Player player)
    {
        player.GetModPlayer<MysteriaPlayer>().active = true;
        player.setBonus = "Increased tile and wall placement speed and range\nIncreased mining speed\nEnemies spawn significantly less often";
        player.pickSpeed -= 0.25f;
        player.tileSpeed += 0.25f;
        player.wallSpeed += 0.25f;
        player.blockRange += 2;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<MysteriaClump>(), 12)
            .AddIngredient(ModContent.ItemType<MysteriaWood>(), 20)
            .AddTile(TileID.Anvils)
            .Register();
    }

    private class MysteriaPlayer : ModPlayer
    {
        internal bool active = false;

        public override void ResetEffects() => active = false;
    }

    private class MysteriaNPC : GlobalNPC 
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.GetModPlayer<MysteriaPlayer>().active)
            {
                spawnRate = (int)(spawnRate * 0.1f);
                maxSpawns = (int)(maxSpawns * 0.2f);
            }
        }
    }
}