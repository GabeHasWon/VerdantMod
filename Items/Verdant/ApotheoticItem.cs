using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Tiles.Verdant.Decor;

namespace Verdant.Items.Verdant;

public abstract class ApotheoticItem : ModItem, IDialogueCache
{
    public abstract ScreenText Dialogue(bool forServer);

    public sealed override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        if (!NPC.downedMoonlord)
            return;

        tooltips.Add(new TooltipLine(Mod, "Verdant:Apotheotic", "Show this to the Apotheosis by right clicking them"));
    }
}

public class ApotheoticGlobalItem : GlobalItem
{
    public override void HoldItem(Item item, Player player) //So I don't need to override ApotheoticItem.HoldItem and call base
    {
        if (!NPC.downedMoonlord || item.ModItem is not ApotheoticItem)
            return;

        bool close = player.InInteractionRange(Player.tileTargetX, Player.tileTargetY);
        Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];

        if (close && tile.HasTile && (tile.TileType == ModContent.TileType<Apotheosis>() || tile.TileType == ModContent.TileType<HardmodeApotheosis>()))
        {
            player.cursorItemIconText = "";
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = item.type;
        }
    }
}