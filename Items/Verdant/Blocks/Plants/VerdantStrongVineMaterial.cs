using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks.Plants
{
    public class VerdantStrongVineMaterial : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Hardy Vine", "'It takes quite the sharp blade to cut through these'");
        public override void SetDefaults() => QuickItem.SetMaterial(this, 16, 16, 0, 999, true);

        public override bool CanUseItem(Player player)
        {
            Point p = Main.MouseWorld.ToTileCoordinates();
            bool c = !Framing.GetTileSafely(p.X, p.Y).HasTile || Main.tileCut[Framing.GetTileSafely(p.X, p.Y).TileType];
            bool a = TileHelper.ActiveType(p.X, p.Y - 1, ModContent.TileType<VerdantGrassLeaves>()) || TileHelper.ActiveType(p.X, p.Y - 1, ModContent.TileType<VerdantStrongVine>());
            return c && a;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (player.whoAmI == Main.myPlayer)
            {
                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, ModContent.TileType<VerdantStrongVine>(), false, false);

                if (Main.netMode != NetmodeID.SinglePlayer)
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, Player.tileTargetX, Player.tileTargetY);
            }
            return true;
        }

        public override void HoldItem(Player player)
        {
            if (CanUseItem(player))
            {
                player.cursorItemIconText = "";
                player.cursorItemIconID = Item.type;
                player.noThrow = 2;
                player.cursorItemIconEnabled = true;
            }
        }
    }
}
