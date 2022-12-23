using Terraria;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic;

namespace Verdant.Items.Verdant.Blocks;

public class WisplantInfusedMudBoulder : ModItem
{
    public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Wisplant-Infused Lush Soil Ball");
		Tooltip.SetDefault("'You just stuck two things together, it's not magic' - Everybody\n'Magic isn't always a spectacle' - Wizards");
	}

	public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<WisplantInfusedLushBall>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, -1, 1, (ModContent.ItemType<LushSoilBlock>(), 1));
}