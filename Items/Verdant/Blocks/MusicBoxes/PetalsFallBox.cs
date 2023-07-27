using Terraria.ModLoader;
using Terraria.ID;
using Verdant.Tiles.Verdant.Decor.MusicBox;

namespace Verdant.Items.Verdant.Blocks.MusicBoxes
{
	[Sacrifice(1)]
	public class PetalsFallBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Music Box (Petals Fall)");
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/PetalsFall"), ModContent.ItemType<PetalsFallBox>(), ModContent.TileType<PetalsFallBoxTile>());
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<PetalsFallBoxTile>();
			Item.width = 34;
			Item.height = 26;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
			Item.hasVanityEffects = true;
		}
	}
}