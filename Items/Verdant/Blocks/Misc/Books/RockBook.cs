using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc.Books;

[Sacrifice(1)]
public class RockBook : ModItem
{
	public override void SetDefaults() => QuickItem.SetBlock(this, 28, 30, ModContent.TileType<SpecialBooks>(), maxStack: 1, createStyle: 6, autoReuse: false);
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

			QuickItem.ToggleBookUI(Language.GetTextValue("Mods.Verdant.Books.RockBook.Title"), 0.75f,
				new object[] { ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/IgneousRock", AssetRequestMode.ImmediateLoad),
                    Language.GetTextValue("Mods.Verdant.Books.RockBook.Content"),
                    ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/Volcano", AssetRequestMode.ImmediateLoad) });
			return true;
		}

		Item.placeStyle = Main.rand.Next(2) + 6;
		return null;
	}
}
