using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Blocks
{
    internal class OvergrownBricks : ModTile, IVerdantGrassTile
    {
        public override void SetStaticDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.Grass, SoundID.Grass, new Color(90, 120, 90), true, false);

            Main.tileBlendAll[Type] = true;
            Main.tileBrick[Type] = true;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail)
            {
                Tile tile = Main.tile[i, j];
                tile.TileType = TileID.GrayBrick;
            }
        }

        public override void RandomUpdate(int i, int j)
        {
            if (VerdantGrassLeaves.StaticRandomUpdate(i, j))
                return;

            TileHelper.Spread(i, j, Type, 4, true, TileID.GrayBrick);
        }
    }
}