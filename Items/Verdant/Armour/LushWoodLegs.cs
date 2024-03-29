using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;

namespace Verdant.Items.Verdant.Armour
{
    [AutoloadEquip(EquipType.Legs)]
    public class LushWoodLegs : ModItem
	{
        public override void SetDefaults()
		{
			Item.width = 18;
            Item.height = 12;
            Item.value = Item.buyPrice(0, 0, 0, 40);
            Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<VerdantWoodBlock>(10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
	}
}