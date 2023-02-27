using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks.Plants
{
    public class RootRopeItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Root Rope", "");

        public override void SetDefaults()
        {
            QuickItem.SetBlock(this, 16, 16, ModContent.TileType<RootRopeTile>(), true);
            Item.useAnimation = Item.useTime = 12;
            Item.tileBoost = 2;
        }

        public override void AddRecipes()
        {
            QuickItem.AddRecipe(this, TileID.WorkBenches, 2, (ModContent.ItemType<LushWood.VerdantWoodBlock>(), 1));
            QuickItem.AddRecipe(ModContent.ItemType<LushWood.VerdantWoodBlock>(), TileID.WorkBenches, 1, (Type, 2));
        }

        public override bool? UseItem(Player player)
        {
            static bool Valid(int x, int y) => Main.tile[x, y].HasTile && !Main.tileCut[Main.tile[x, y].TileType];

            var m = Main.MouseWorld.ToTileCoordinates();
            Tile tile = Main.tile[m];
            if (tile.HasTile && tile.TileType != Item.createTile || !Valid(m.X, m.Y - 1) && !Valid(m.X, m.Y + 1))
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
