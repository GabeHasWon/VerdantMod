using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
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

			QuickItem.ToggleBookUI("Hardy Vines", 0.8f,
				new object[] { ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/HardyVine", AssetRequestMode.ImmediateLoad),
                "\n\"The final shreds of life around here\nIncredibly durable, despite how thin they are.\nTrying to cut one of these apart is borderline futile\n" +
                "Grow sparsely, surrounded by more average\nvines and fauna.\nSometimes flower, with small light bulbs on them.\n\nDO NOT EAT\n\nLike lettuce but leathery and dirtier.\n" +
                "Can be used to create incredibly sturdy ropes.\nLook into use as a reinforcement?\n\nI don't know if I can\"\nThe few remaining pages have small sketches and\nthe occasional small note -" +
                "\n\"More plentiful now?\"\n\"New specimens are getting thicker...\"\n\n - until the book ends."});
			return true;
		}

		Item.placeStyle = Main.rand.Next(2) + 4;
		return null;
	}
}
