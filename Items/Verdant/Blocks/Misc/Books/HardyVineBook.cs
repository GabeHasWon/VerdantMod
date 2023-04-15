using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc.Books;

public class HardyVineBook : ModItem
{
	public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Hardy Vine Research", "'An impressive amount of insight into hardy vines'\nRight click to read");
	public override void SetDefaults() => QuickItem.SetBlock(this, 28, 30, ModContent.TileType<SpecialBooks>(), maxStack: 1, createStyle: 4, autoReuse: false);
	public override bool AltFunctionUse(Player player) => true;

	public override bool? UseItem(Player player)
	{
		if (player.altFunctionUse == 2 && player.itemAnimation > 14)
		{
			Item.stack++;

			QuickItem.ToggleBookUI(Language.GetTextValue("Mods.Verdant.Books.HardyVineBook.Title"), 0.8f,
				new object[] { ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/HardyVine", AssetRequestMode.ImmediateLoad),
                Language.GetTextValue("Mods.Verdant.Books.HardyVineBook.Content")});
			return true;
		}

		Item.placeStyle = Main.rand.Next(2) + 4;
		return null;
	}
}
