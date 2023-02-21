using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.UI;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc;

public class LeafBook : ModItem
{
	public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Leaf Research", "'An impressive amount of insight into lush leaves'\nRight click to read");
	public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<SpecialBooks>(), maxStack: 1, createStyle: 2, autoReuse: false);
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
				ModContent.GetInstance<UISystem>().BookInterface.SetState(new BookState("Leaves", 1f, 
					new object[] { ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/LeafDisplay", AssetRequestMode.ImmediateLoad), 
					"\nAlmost fabric like texture, durable yet soft\ninexplicably calming to hold\nGrows beautifully in bulb light",
					ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/LightbulbCrossSection", AssetRequestMode.ImmediateLoad), 
					"\nunpleasant to eat, tastes like grass and dirt\nPlentiful, might use (books? pages? look soon)"}));
			}
			return true;
		}

		Item.placeStyle = Main.rand.Next(2) + 2;
		return null;
	}
}
