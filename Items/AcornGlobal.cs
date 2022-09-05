using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items
{
    class AcornGlobal : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Acorn;

        public override bool? UseItem(Item item, Player player)
        {
            Point p = Main.MouseWorld.ToTileCoordinates();
            Tile tile = Main.tile[p.X, p.Y + 1];

            if (player.IsInTileInteractionRange(p.X, p.Y + 1) && tile.HasTile && tile.TileType == ModContent.TileType<VerdantGrassLeaves>() && !WorldGen.SolidTile(p.X, p.Y) && !WorldGen.SolidTile(p.X, p.Y - 1) && Main.tile[p.X, p.Y].TileType != ModContent.TileType<LushSapling>())
            {
                WorldGen.PlaceTile(p.X, p.Y, ModContent.TileType<LushSapling>());
                return true;
            }
            return null;
        }
    }
}
