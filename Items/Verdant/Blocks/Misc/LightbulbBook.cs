using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.UI;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc;

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
				"\nWeird bulbs? Seeds?\nmade of soft plantlike material, like leaves\nGlows softly, though sometimes in a wave motion\nWorks as a source of sunlight, grows on certain flowers",
				ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/LightbulbCrossSection", AssetRequestMode.ImmediateLoad),
				"\nFull of water? sugar?\ntastes bland, a bit sweet, bit of dirt, mildly grassy, and\na tinge of iron (or other metal? test later)\n\nRare, don't have too much to test on\n" +
				"go searching for more later, might be good food\n\nbeyond analysis, these are incredible specimens\nWill make for better light than these fireflies"});
			return true;
		}

		Item.placeStyle = Main.rand.Next(2);
		return null;
	}
}
