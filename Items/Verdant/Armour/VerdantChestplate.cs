using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Armour
{
    [AutoloadEquip(EquipType.Body)]
    public class VerdantChestplate : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Growth Chestpiece");
            Tooltip.SetDefault("+7% minion damage\n+1 max minion");
        }

        public override void SetDefaults()
		{
			item.width = 28;
			item.height = 22;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
            item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
			player.minionDamage *= 1.07f;
            player.maxMinions++;
        }

        public override void AddRecipes()
        {
            ModRecipe m = new ModRecipe(mod);
            m.AddIngredient(ModContent.ItemType<VerdantStrongVineMaterial>(), 6);
            m.AddIngredient(ModContent.ItemType<PinkPetal>(), 14);
            m.AddIngredient(ModContent.ItemType<LushLeaf>(), 20);
            m.AddIngredient(ModContent.ItemType<YellowBulb>(), 2);
            m.AddTile(TileID.Anvils);
            m.SetResult(this);
            m.AddRecipe();
        }
    }
}