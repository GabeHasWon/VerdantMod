using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;

namespace Verdant.Items.Verdant.Armour
{
    [AutoloadEquip(EquipType.Body)]
    public class LushWoodBody : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lush Wood Chestpiece");
            Tooltip.SetDefault("+2 flat minion damage\n+1 max minion");
        }

        public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 22;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon).Flat += 2;
            player.maxMinions++;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<VerdantWoodBlock>(16)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}