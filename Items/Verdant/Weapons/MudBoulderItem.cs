using Terraria;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Weapons;

public class MudBoulderItem : ModItem
{
    public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Lush Soil Ball");
		Tooltip.SetDefault("A minor nuisance");
	}

	public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<MudBoulderTile>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<LushSoilBlock>(), 1));
}