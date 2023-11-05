using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Global;

class AcornGlobal : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Acorn;

    public override void HoldItem(Item item, Player player) 
        => item.createTile = CanPlaceAt(Main.MouseWorld.ToTileCoordinates(), player) ? ModContent.TileType<LushSapling>() : TileID.Saplings;

    public override bool? UseItem(Item item, Player player)
    {
        if (item.createTile == ModContent.TileType<LushSapling>())
        {
            Point p = Main.MouseWorld.ToTileCoordinates();
            Tile cur = Main.tile[p.X, p.Y];
            Tile top = Main.tile[p.X, p.Y - 1];

            if ((!cur.HasTile || Main.tileCut[cur.TileType]) && (!top.HasTile || Main.tileCut[top.TileType]))
            {
                WorldGen.PlaceTile(p.X, p.Y, ModContent.TileType<LushSapling>());
                return true;
            }
        }
        return null;
    }

    /// <summary>
    /// Whether a sapling can be planted here. Checks the tile below the given coordinates.
    /// </summary>
    public static bool CanPlaceAt(Point pos, Player player)
    {
        Tile tile = Main.tile[pos.X, pos.Y + 1];
        bool inRange = player.IsInTileInteractionRange(pos.X, pos.Y + 1, TileReachCheckSettings.Simple);

        return inRange && tile.HasTile && TileObjectData.GetTileData(ModContent.TileType<LushSapling>(), 0).AnchorValidTiles.Contains(tile.TileType)
            && Main.tile[pos].TileType != ModContent.TileType<LushSapling>();
    }
}
