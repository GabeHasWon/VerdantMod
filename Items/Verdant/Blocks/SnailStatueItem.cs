using Terraria;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Decor;

namespace Verdant.Items.Verdant.Blocks;

public class SnailStatueItem : ModItem
{
    public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Snail Statue");
		Tooltip.SetDefault("A statue of a large snail\nThere's a plaque on the bottom");
	}

	public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<SnailStatue>());
}