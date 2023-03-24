using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc.Books;

public class RockBook : ModItem
{
	public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Rock Research", "'Research into...volcanic rocks?'\nRight click to read");
	public override void SetDefaults() => QuickItem.SetBlock(this, 28, 30, ModContent.TileType<SpecialBooks>(), maxStack: 1, createStyle: 6, autoReuse: false);
	public override bool AltFunctionUse(Player player) => true;

	public override bool? UseItem(Player player)
	{
		if (player.altFunctionUse == 2 && player.itemAnimation > 14)
		{
			Item.stack++;

			QuickItem.ToggleBookUI("Volcanic Rock", 0.75f,
				new object[] { ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/IgneousRock", AssetRequestMode.ImmediateLoad),
				"\n\"Supposedly from a distant island, unknown\nWould make for an incredible research opportunity\nStriations of obsidian line the rock\nEmber particles throughout? Research",
				"\nVarious hints of iron, bismite, lead\nand obsidian in no particular order\nDespite its age, still warm to the touch - also sounds hollow?\nNeed more samples, asked around",
				"Might go searching for more\nIt's been a while\nMap shows a volcanic area around [scratch mark] or so...\"\n\nDespite the introduction, this book seems to slowly\nchange in purpose " +
                "- becoming sort of an explorer's log:\n\"Adventure going well,\nthough fruitless; idyll forests\nsit patiently, as\nwhat awaits me at\nthe end of this adventure -\n" +
                "a great volcano\nI'm just sure of it.\"", ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/Volcano", AssetRequestMode.ImmediateLoad) });
			return true;
		}

		Item.placeStyle = Main.rand.Next(2) + 6;
		return null;
	}
}
