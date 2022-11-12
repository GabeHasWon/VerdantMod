using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles;

namespace Verdant.Items.Verdant.Tools;

[Sacrifice(20)]
class Halfsprout : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Halfsprout");
        Tooltip.SetDefault("Instantly grows all herbs, harvests them, then replants them\n");
    }

    public override void SetDefaults()
    {
        Item.Size = new Vector2(40, 56);
        Item.useStyle = ItemUseStyleID.Swing;
        Item.rare = ItemRarityID.Green;
        Item.useTurn = true;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.autoReuse = true;
        Item.consumable = true;
        Item.maxStack = 99;
    }

    public override bool? UseItem(Player player)
    {
        Point mouse = Main.MouseWorld.ToTileCoordinates();
        (int x, int y) = (mouse.X, mouse.Y);

        Tile tile = Main.tile[x, y];

        if (tile.HasTile && IsTileAnHerb(tile.TileType, out bool vanilla))
        {
            if (vanilla)
            {
                short frameX = tile.TileFrameX;

                tile.TileType = TileID.MatureHerbs;
                TileHelper.SyncedKill(x, y);
                tile.HasTile = true;
                tile.TileType = TileID.ImmatureHerbs;
                tile.TileFrameX = frameX;
                tile.TileFrameY = 0;

                if (Main.netMode != NetmodeID.SinglePlayer)
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y, 0f, 0, 0, 0);
            }
            return true;
        }
        return false;
    }

    private bool IsTileAnHerb(int type, out bool vanillaHerb)
    {
        vanillaHerb = false;
        bool isHerb = type == TileID.BloomingHerbs || type == TileID.ImmatureHerbs || type == TileID.MatureHerbs;

        if (isHerb)
            vanillaHerb = true;
        return isHerb;
    }
}
