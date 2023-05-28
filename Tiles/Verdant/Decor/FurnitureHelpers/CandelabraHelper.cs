using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles.Verdant.Decor;

public static class CandelabraHelper
{
    public static void Defaults(ModTile tile, Color color, bool cantPlaceInWater = true)
    {
        int type = tile.Type;
        Main.tileLighted[type] = true;
        Main.tileFrameImportant[type] = true;
        Main.tileNoAttach[type] = true;
        Main.tileWaterDeath[type] = true;
        Main.tileLavaDeath[type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);

        if (cantPlaceInWater)
        {
            TileObjectData.newTile.WaterDeath = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
        }

        TileObjectData.addTile(type);

        tile.AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
        tile.AddMapEntry(color, Language.GetText("ItemName.Candelabra"));
    }

    public static void WireHit(int i, int j)
    {
        Tile tile = Main.tile[i, j];

        int leftX = i - tile.TileFrameX / 18 % 2;
        int topY = j - tile.TileFrameY / 18 % 2;
        short frameAdjustment = (short)(tile.TileFrameX < 36 ? 36 : -36);

        for (int k = 0; k < 2; ++k)
        {
            for (int b = 0; b < 2; ++b)
            {
                Main.tile[leftX + k, topY + b].TileFrameX += frameAdjustment;
                Wiring.SkipWire(leftX + k, topY + b);
            }
        }

        NetMessage.SendTileSquare(-1, leftX, topY + 1, 1, TileChangeType.None);
    }
}
