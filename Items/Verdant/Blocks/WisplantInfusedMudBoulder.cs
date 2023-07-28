using Terraria;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks;

public class WisplantInfusedMudBoulder : ModItem
{
	public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<WisplantInfusedLushBall>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<LushSoilBlock>(), 1), (ModContent.ItemType<WisplantItem>(), 1));
}