using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc.Books;

[Sacrifice(1)]
public class PoetryBook : ModItem
{
	public override void SetDefaults() => QuickItem.SetBlock(this, 28, 32, ModContent.TileType<SpecialBooks>(), rarity: ItemRarityID.Purple, maxStack: 1, createStyle: 7, autoReuse: false);
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

            QuickItem.ToggleBookUI(Language.GetTextValue("Mods.Verdant.Books.PoetryBook.Title"), 0.8f,
                [Language.GetTextValue("Mods.Verdant.Books.PoetryBook.Content")]);
			return true;
		}

		Item.placeStyle = 7 + Main.rand.Next(2);
		return null;
	}
}
