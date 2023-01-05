using Terraria.ModLoader;
using Terraria.ID;
using Verdant.Tiles.Verdant.Decor.MusicBox;

namespace Verdant.Items.Verdant.Blocks.MusicBoxes
{
	[Sacrifice(1)]
	public class VibrantHorizonBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Vibrant Horizon)");
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/VibrantHorizon"), ModContent.ItemType<VibrantHorizonBox>(), ModContent.TileType<VibrantHorizonBoxTile>());
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<VibrantHorizonBoxTile>();
			Item.width = 32;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
			Item.canBePlacedInVanityRegardlessOfConditions = true;
		}
	}
}