using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc.Books;

[Sacrifice(1)]
public class RockBook : ApotheoticItem
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

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(RockBook))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.RockBook.", 4, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.RockBook.0").
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.RockBook.1")).
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.RockBook.2")).
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.RockBook.3"));
    }
}
