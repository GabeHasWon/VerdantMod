using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc.Books;

public class RockBook : ModItem
{
	public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Rock Research", "'Research into...volcanic rocks?'\nRight click to read");
	public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<SpecialBooks>(), maxStack: 1, createStyle: 0, autoReuse: false);
	public override bool AltFunctionUse(Player player) => true;

	public override bool? UseItem(Player player)
	{
		if (player.altFunctionUse == 2 && player.itemAnimation > 14)
		{
			Item.stack++;

			QuickItem.ToggleBookUI("\"Lightbulbs\"", 0.8f,
				new object[] { ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/LightbulbDisplay", AssetRequestMode.ImmediateLoad),
				"\n\"Supposedly from a distant island, unknown\nWould make for an incredible research opportunity\nStriations of obsidian line the rock\nEmber particles throughout? Research",
				ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/LightbulbCrossSection", AssetRequestMode.ImmediateLoad),
				"\nVarious hints of iron, bismite, lead\nand obsidian in no particular order\nDespite its age, still warm to the touch - also sounds hollow?\nNeed more samples, asked around",
				"Might go searching for more\nIt's been a while\nMap shows a volcanic area around [scratch mark] or so...\"\nDespite the purpose, this book seems to slowly\nchange in purpose " +
                "- becoming sort of an explorer's log:\n\"Exploration going well, though fruitless\nIdyll forests sit patiently, though I've no interest in them\n"});
			return true;
		}

		Item.placeStyle = Main.rand.Next(2);
		return null;
	}
}
