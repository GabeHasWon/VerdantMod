using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items
{
    class VerdantGlobalItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Acorn;

        public override void HoldItem(Item item, Player player)
        {
            Point p = Main.MouseWorld.ToTileCoordinates();
            Tile tile = Main.tile[p.X, p.Y + 1];

            if (player.IsInTileInteractionRange(p.X, p.Y + 1) && tile.HasTile && tile.TileType == ModContent.TileType<VerdantGrassLeaves>())
            {
            }
        }

        public override bool? UseItem(Item item, Player player)
        {
            Point p = Main.MouseWorld.ToTileCoordinates();
            Tile tile = Main.tile[p.X, p.Y + 1];

            if (player.IsInTileInteractionRange(p.X, p.Y + 1) && tile.HasTile && tile.TileType == ModContent.TileType<VerdantGrassLeaves>())
            {
                WorldGen.PlaceTile(p.X, p.Y, ModContent.TileType<LushSapling>());
                return true;
            }
            return null;
        }
    }
}
