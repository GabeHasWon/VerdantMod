using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles.Verdant.Decor;

internal static class BedHelper
{
    public static void Defaults(ModTile tile, Color color)
    {
        int type = tile.Type;
        Main.tileFrameImportant[type] = true;
        Main.tileLavaDeath[type] = true;

        TileID.Sets.HasOutlines[type] = true;
        TileID.Sets.CanBeSleptIn[type] = true;
        TileID.Sets.InteractibleByNPCs[type] = true;
        TileID.Sets.IsValidSpawnPoint[type] = true;
        TileID.Sets.DisableSmartCursor[type] = true;

        tile.AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

        tile.DustType = DustID.RichMahogany;
        tile.AdjTiles = new int[] { TileID.Beds };

        TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
        TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
        TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
        TileObjectData.addTile(type);

        tile.AddMapEntry(color, Language.GetText("ItemName.Bed"));
    }

    public static bool RightClick(int i, int j)
    {
        Player player = Main.LocalPlayer;
        Tile tile = Main.tile[i, j];

        int spawnX = i - (tile.TileFrameX / 18) + (tile.TileFrameX >= 72 ? 5 : 2);
        int spawnY = j + 1;

        if (!Player.IsHoveringOverABottomSideOfABed(i, j))
        {
            if (player.IsWithinSnappngRangeToTile(i, j, PlayerSleepingHelper.BedSleepingMaxDistance))
            {
                player.GamepadEnableGrappleCooldown();
                player.sleeping.StartSleeping(player, i, j);
            }
        }
        else
        {
            player.FindSpawn();

            if (player.SpawnX == spawnX && player.SpawnY == spawnY)
            {
                player.RemoveSpawn();
                Main.NewText(Language.GetTextValue("Game.SpawnPointRemoved"), byte.MaxValue, 240, 20);
            }
            else if (Player.CheckSpawn(spawnX, spawnY))
            {
                player.ChangeSpawn(spawnX, spawnY);
                Main.NewText(Language.GetTextValue("Game.SpawnPointSet"), byte.MaxValue, 240, 20);
            }
        }
        return true;
    } 
}
