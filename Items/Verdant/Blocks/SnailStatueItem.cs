using Terraria;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Decor;

namespace Verdant.Items.Verdant.Blocks;

public class SnailStatueItem : ModItem
{
	public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<SnailStatue>());
}