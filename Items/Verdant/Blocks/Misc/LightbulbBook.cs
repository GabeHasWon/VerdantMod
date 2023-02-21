using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.UI;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc;

public class LightbulbBook : ModItem
{
	public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lightbulb Research", "'An impressive amount of inight into light bulbs'");
	public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<SpecialBooks>(), maxStack: 1, createStyle: 0, autoReuse: false);
	public override bool AltFunctionUse(Player player) => true;

	public override bool? UseItem(Player player)
	{
		if (player.altFunctionUse == 2 && player.itemAnimation > 14)
		{
			Item.stack++;

			if (ModContent.GetInstance<UISystem>().BookInterface.CurrentState is not null)
			{
				SoundEngine.PlaySound(SoundID.MenuClose);
				ModContent.GetInstance<UISystem>().BookInterface.SetState(null);
			}
			else
			{
				SoundEngine.PlaySound(SoundID.MenuOpen);
				ModContent.GetInstance<UISystem>().BookInterface.SetState(new BookState("Lightbulbs", 0.8f, 
					new object[] { "-------", ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/LightbulbDisplay"), 
					"\nCurious bulbs?\nMade of soft plantlike material, like leaves\nGlows softly, though sometimes in a wave motion\nWorks as a source of sunlight, grows on certain flowers",
					ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/LightbulbCrossSection"), "\nFull of water (sugar water?)\nTaste like absolutely nothing, a bit sweet, bit of dirt, DEEPLY" +
                    " grassy, and a tinge of iron (or other metal? test later)"}));
			}
			return true;
		}

		Item.placeStyle = Main.rand.Next(2);
		return null;
	}
}
