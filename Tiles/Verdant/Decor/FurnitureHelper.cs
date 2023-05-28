using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles.Verdant.Decor;

public static class FurnitureHelper
{
    public static bool ChairInteract(int i, int j, SmartInteractScanSettings settings) => settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance); // Avoid being able to trigger it from long range

    public static void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info, int nextStyleHeight = 40)
    {
        Tile tile = Framing.GetTileSafely(i, j);

        info.TargetDirection = -1;
        if (tile.TileFrameX != 0)
            info.TargetDirection = 1;

        info.AnchorTilePosition.X = i;
        info.AnchorTilePosition.Y = j;

        if (tile.TileFrameY % nextStyleHeight == 0)
            info.AnchorTilePosition.Y++;
    }

    public static bool RightClick(int i, int j)
    {
        Player player = Main.LocalPlayer;

        if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance)) // Avoid being able to trigger it from long range
        {
            player.GamepadEnableGrappleCooldown();
            player.sitting.SitDown(player, i, j);
        }

        return true;
    }

    public static void MouseOver(int i, int j, int itemType)
    {
        Player player = Main.LocalPlayer;

        if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance)) // Match condition in RightClick. Interaction should only show if clicking it does something
            return;

        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
        player.cursorItemIconID = itemType;

        if (player.direction < 1)
            player.cursorItemIconReversed = true;
    }

    public static void CandleDefaults(ModTile tile, Color color, bool cantPlaceInWater = true)
    {
        int type = tile.Type;
        Main.tileLighted[type] = true;
        Main.tileFrameImportant[type] = true;
        Main.tileNoAttach[type] = true;
        Main.tileWaterDeath[type] = true;
        Main.tileLavaDeath[type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);

        if (cantPlaceInWater)
        {
            TileObjectData.newTile.WaterDeath = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
        }

        TileObjectData.addTile(type);

        tile.AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
        tile.AddMapEntry(color, Language.GetText("ItemName.Candle"));
    }
}
