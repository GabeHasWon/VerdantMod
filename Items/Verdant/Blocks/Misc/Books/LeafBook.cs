using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc.Books;

public class LeafBook : ModItem
{
	public override void SetDefaults() => QuickItem.SetBlock(this, 28, 32, ModContent.TileType<SpecialBooks>(), maxStack: 1, createStyle: 2, autoReuse: false);
	public override bool AltFunctionUse(Player player) => true;

    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse == 2)
            Item.createTile = -1;
        else
            Item.createTile = ModContent.TileType<SpecialBooks>();

        return true;
    }

    public override bool? UseItem(Player player)
	{
		if (player.altFunctionUse == 2 && player.itemAnimation > 14)
		{
			Item.stack++;

			QuickItem.ToggleBookUI(Language.GetTextValue("Mods.Verdant.Books.LeafBook.Title"), 1f,
				new object[] { Language.GetTextValue("Mods.Verdant.Books.LeafBook.Content.0"),
				ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/Signature", ReLogic.Content.AssetRequestMode.ImmediateLoad),
                Language.GetTextValue("Mods.Verdant.Books.LeafBook.Content.1"),
				ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/LeafDisplay", ReLogic.Content.AssetRequestMode.ImmediateLoad),
                Language.GetTextValue("Mods.Verdant.Books.LeafBook.Content.2")});
			return true;
		}

		Item.placeStyle = Main.rand.Next(2) + 2;
		return null;
	}
}
