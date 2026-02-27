using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Misc.Apotheotic;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc.Books;

[Sacrifice(1)]
public class HardyVineBook : ApotheoticItem
{
	public override void SetDefaults() => QuickItem.SetBlock(this, 28, 30, ModContent.TileType<SpecialBooks>(), maxStack: 1, createStyle: 4, autoReuse: false);
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

			QuickItem.ToggleBookUI(Language.GetTextValue("Mods.Verdant.Books.HardyVineBook.Title"), 0.8f,
				new object[] { ModContent.Request<Texture2D>("Verdant/Systems/UI/Textures/HardyVine", AssetRequestMode.ImmediateLoad),
                Language.GetTextValue("Mods.Verdant.Books.HardyVineBook.Content")});
			return true;
		}

		Item.placeStyle = Main.rand.Next(2) + 4;
		return null;
	}

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(HardyVineBook))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.HardyVineBook.", 2, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.HardyVineBook.0").
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.HardyVineBook.1"));
    }
}
