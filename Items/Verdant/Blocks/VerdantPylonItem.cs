using Terraria.Enums;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Decor;

namespace Verdant.Items.Verdant.Blocks
{
	[Sacrifice(1)]
	internal class VerdantPylonItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Verdant Pylon");

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<VerdantPylonTile>());
			Item.SetShopValues(ItemRarityColor.Blue1, Terraria.Item.buyPrice(gold: 10));
		}
	}
}
