using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc.Books;

public class LightbulbBook : ModItem
{
	public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lightbulb Research", "'An impressive amount of insight into light bulbs'\nRight click to read");
	public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<SpecialBooks>(), maxStack: 1, createStyle: 0, autoReuse: false);
	public override bool AltFunctionUse(Player player) => true;

	public override bool? UseItem(Player player)
	{
		if (player.altFunctionUse == 2 && player.itemAnimation > 14)
		{
			Item.stack++;

			QuickItem.ToggleBookUI("\"Lightbulbs\"", 0.8f,
				new object[] { ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/LightbulbDisplay", AssetRequestMode.ImmediateLoad),
				"\n\"Weird bulbs? Seeds?\nmade of soft plantlike material, like leaves\nGlows softly, though sometimes in a...wave like motion\nWorks as a source of sunlight, grows on certain flowers",
				ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/LightbulbCrossSection", AssetRequestMode.ImmediateLoad),
				"\nFull of water? sugar?\ntastes bland, a bit sweet, bit of dirt, mildly grassy, and\na tinge of iron (or other metal? test later)\nRare, don't have too much to test on\n" +
				"Go searching for more later, might be good food\nBeyond analysis, these are incredible specimens\nWill make for better light than these fireflies\"\n\nRandom things line pages" +
                ": scribbles, sketches, notes\nOccasionally there's a small tidbit of salvageable info:\n\"Grows better in dirty water?\"\n\"How would this work in sunlight?\"\n\"Almost flowers when growing\"" +
                "\n\"Soft, but hardy\""});
			return true;
		}

		Item.placeStyle = Main.rand.Next(2);
		return null;
	}
}
