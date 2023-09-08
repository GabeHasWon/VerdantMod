using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.Plants;

public class LushGrassSeeds : ModItem
{
    public override void SetDefaults()
    {
        Item.autoReuse = true;
        Item.useTurn = true;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.rare = ItemRarityID.White;
        Item.useTime = 15;
        Item.maxStack = Item.CommonMaxStack;
        Item.width = 16;
        Item.height = 18;
        Item.consumable = true;
    }

    public override bool? UseItem(Player player)
    {
        Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);

        if (tile.HasTile && tile.TileType == ModContent.TileType<LushSoil>() && player.InInteractionRange(Player.tileTargetX, Player.tileTargetY, TileReachCheckSettings.Simple))
        {
            tile.TileType = (ushort)ModContent.TileType<LushGrass>();
            WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1, TileChangeType.None);
            return true;
        }
        return false;
    }
}