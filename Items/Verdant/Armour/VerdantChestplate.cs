using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Armour
{
    [AutoloadEquip(EquipType.Body)]
    public class VerdantChestplate : ModItem
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Growth Chestpiece");
            // Tooltip.SetDefault("+7% minion damage\n+1 max minion");
        }

        public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 22;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
			player.GetDamage(DamageClass.Summon) *= 1.07f;
            player.maxMinions++;
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
}