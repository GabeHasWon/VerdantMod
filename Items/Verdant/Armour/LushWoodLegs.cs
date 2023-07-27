using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;

namespace Verdant.Items.Verdant.Armour
{
    [AutoloadEquip(EquipType.Legs)]
    public class LushWoodLegs : ModItem
	{
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Lush Wood Leggings");

        public override void SetDefaults()
		{
			Item.width = 18;
            Item.height = 12;
			Item.value = 10000;
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