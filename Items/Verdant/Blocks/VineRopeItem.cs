using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks
{
    public class VineRopeItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Vine Rope", "");

        public override void SetDefaults()
        {
            QuickItem.SetBlock(this, 16, 16, ModContent.TileType<VineRope>(), true);
            Item.useAnimation = Item.useTime = 6;
            Item.tileBoost = 3;
        }

        public override bool? UseItem(Player player)
        {
            Tile tile = Main.tile[Main.MouseWorld.ToTileCoordinates()];
            if (tile.HasTile && tile.TileType != Item.createTile)
                return false;

            if (!tile.HasTile)
                return true;
            else
            {
                int x = (int)(Main.MouseWorld.X / 16f);
                int y = (int)(Main.MouseWorld.Y / 16f);

                while (Main.tile[x, y].HasTile && Main.tile[x, y].TileType == Item.createTile)
                    y++;

                if (!Main.tile[x, y].HasTile) //If we can place here, do it
                    return true;

                y--;
                while (Main.tile[x, y].HasTile)
                    y--;

                if (!Main.tile[x, y].HasTile && Main.tile[x, y + 1].TileType == Item.createTile)
                {
                    TileHelper.SyncedPlace(x, y, Item.createTile, false);
                    return true;
                }
                else
                    return false;
            }
        }
    }
}
