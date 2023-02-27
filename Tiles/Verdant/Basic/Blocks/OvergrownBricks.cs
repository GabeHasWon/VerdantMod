using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Misc;

namespace Verdant.Tiles.Verdant.Basic.Blocks
{
    internal class OvergrownBricks : ModTile
    {
        public override void SetStaticDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.Grass, SoundID.Grass, new Color(90, 120, 90), ModContent.ItemType<OvergrownBrickItem>(), "", true, false);

            Main.tileBlendAll[Type] = true;
            Main.tileBrick[Type] = true;

            VerdantGrassLeaves.CountsAsVerdantGrass.Add(nameof(Verdant) + "." + nameof(OvergrownBricks));
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            fail = true;

            Tile tile = Main.tile[i, j];
            tile.TileType = TileID.GrayBrick;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (VerdantGrassLeaves.StaticRandomUpdate(i, j))
                return;

            TileHelper.Spread(i, j, Type, 4, TileID.GrayBrick);
        }
    }
}