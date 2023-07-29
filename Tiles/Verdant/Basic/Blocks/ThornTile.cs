using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.PestControl;

namespace Verdant.Tiles.Verdant.Basic.Blocks;

internal class ThornTile : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.Ash, SoundID.DD2_SkeletonHurt, new Color(68, 62, 50), "", true, false);
        Main.tileBrick[Type] = true;
    }

    public override void RandomUpdate(int i, int j)
    {
        if (Main.rand.NextBool(4) && !Main.tile[i, j - 1].HasTile && !Main.tile[i, j - 2].HasTile && !Main.tile[i, j].TopSlope)
        {
            WorldGen.PlaceTile(i, j - 2, ModContent.TileType<ThornDecor1x2>(), true, false, -1, Main.rand.Next(4));

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendTileSquare(-1, i, j - 2, 1, 2, TileChangeType.None);
        }

        if (Main.rand.NextBool(2) && !Main.tile[i, j - 1].HasTile && !Main.tile[i, j].TopSlope)
        {
            WorldGen.PlaceTile(i, j - 1, ModContent.TileType<ThornDecor1x1>(), true, false, -1, Main.rand.Next(4));

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendTileSquare(-1, i, j - 1, 1, TileChangeType.None);
        }
    }
}