using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
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

			QuickItem.ToggleBookUI($"[i:{Type}] Leaves [i:{Type}]", 1f,
				new object[] { "The book looks particularly old, page worn and yellowed.\nThe material is also clearly foreign.\nThere's drops of water across the top pages.\nThere's also tons of " +
                "minor scribbles.\nThe scribbles seem of random mundane cave objects,\nfor some reason.\nThere's no dates or authors mentioned, apart from a\nsignature:",
				ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/Signature"),
				"\nYou scan through the book, and spot the following pages:",
				ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/LeafDisplay"),
				"\n\"Almost fabric like texture, durable yet soft.\nInexplicably calming to hold, grows well in bulb light.\nUnpleasant to eat, tastes like grass and dirt." +
                "\nPlentiful, might use (books? pages? look soon).\nMight work in small parts?\nGrows in vine like clumps, or just clumps? Unsure.\nGrow only or mostly around flowers." +
                "\n\n(DO NOT EAT)\n\nTried pressing into pages, seem to work but needs\nmore work. Pages are too green.\""});
			return true;
		}

		Item.placeStyle = Main.rand.Next(2) + 2;
		return null;
	}
}
