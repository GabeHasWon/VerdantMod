using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Tiles.Verdant.Decor;

namespace Verdant.Items;

public abstract class ApotheoticItem : ModItem, IDialogueCache
{
    public abstract ScreenText Dialogue(bool forServer);

    public override void ModifyTooltips(List<TooltipLine> tooltips)
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

        bool close = player.InInteractionRange(Player.tileTargetX, Player.tileTargetY, TileReachCheckSettings.Simple);
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