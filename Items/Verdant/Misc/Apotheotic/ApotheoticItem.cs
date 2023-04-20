using Terraria;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Tiles.Verdant.Decor;

namespace Verdant.Items.Verdant.Misc.Apotheotic;

public abstract class ApotheoticItem : ModItem, IDialogueCache
{
    public abstract ScreenText Dialogue(bool forServer);

    public sealed override void HoldItem(Player player)
    {
        bool close = player.InInteractionRange(Player.tileTargetX, Player.tileTargetY);
        Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];

        if (close && tile.HasTile && (tile.TileType == ModContent.TileType<Apotheosis>() || tile.TileType == ModContent.TileType<HardmodeApotheosis>()))
        {
            player.cursorItemIconText = "";
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = Type;
        }
    }
}
