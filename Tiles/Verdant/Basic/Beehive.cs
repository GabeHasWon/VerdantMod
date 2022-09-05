using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic
{
    class Beehive : ModTile
    {
        const int FrameHeight = 38;

        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>() };
            TileObjectData.newTile.StyleHorizontal = true;

            QuickTile.SetMulti(this, 2, 2, DustID.Bee, SoundID.Dig, true, new Color(143, 21, 193));
        }

        public override void HitWire(int i, int j)
        {
            Tile t = Main.tile[i, j];

            if (t.TileFrameY < FrameHeight)
                return;

            if (t.TileFrameY < FrameHeight * 2 && false)
                WorldGen.PlaceLiquid(i, j, LiquidID.Honey, 128);
            else
                WorldGen.PlaceLiquid(i, j, LiquidID.Honey, 255);

            Point tL = TileHelper.GetTopLeft(new Point(i, j));
            ResetFrame(tL);
        }

        public override bool RightClick(int i, int j)
        {
            Tile t = Main.tile[i, j];

            if (t.TileFrameY < FrameHeight)
                return false;

            HitWire(i, j);
            return true;
        }

        private static void ResetFrame(Point tL)
        {
            for (int i = tL.X; i < tL.X + 2; ++i)
            {
                for (int j = tL.Y; j < tL.Y + 2; ++j)
                {
                    Tile t = Main.tile[i, j];
                    t.TileFrameY = (short)(j == tL.Y ? 0 : 18);
                }
            }
        }

        internal static void IncreaseFrame(Point tL)
        {
            if (Main.tile[tL.X, tL.Y].TileFrameY >= FrameHeight * 2)
                return;

            for (int i = tL.X; i < tL.X + 2; ++i)
            {
                for (int j = tL.Y; j < tL.Y + 2; ++j)
                {
                    Tile t = Main.tile[i, j];
                    t.TileFrameY += FrameHeight;
                }
            }
        }
    }
}