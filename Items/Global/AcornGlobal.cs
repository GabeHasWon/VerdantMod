using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Global;

class AcornGlobal : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Acorn;

    public override bool? UseItem(Item item, Player player)
    {
        Point p = Main.MouseWorld.ToTileCoordinates();
        Tile tile = Main.tile[p.X, p.Y + 1];

        if (player.IsInTileInteractionRange(p.X, p.Y + 1) && tile.HasTile && tile.TileType == ModContent.TileType<VerdantGrassLeaves>() && Main.tile[p.X, p.Y].TileType != ModContent.TileType<LushSapling>())
        {
            Tile top = Main.tile[p.X, p.Y - 1];
            Tile bot = Main.tile[p.X, p.Y];

            if ((!top.HasTile || Main.tileCut[top.TileType]) && (!bot.HasTile || Main.tileCut[bot.TileType]))
            {
                WorldGen.PlaceTile(p.X, p.Y, ModContent.TileType<LushSapling>());
                return true;
            }
        }
        return null;
    }
}
