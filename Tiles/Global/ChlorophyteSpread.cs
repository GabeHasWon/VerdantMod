using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Global;

internal class ChlorophyteSpread : GlobalTile
{
    public override void RandomUpdate(int i, int j, int type)
    {
        if (NPC.downedPlantBoss && type == TileID.Chlorophyte && WorldGen.AllowedToSpreadInfections && Chlorophyte(i, j))
        {
            if (TileHelper.Spread(i, j, type, 2, false, new int[] { ModContent.TileType<LushSoil>(), TileID.Mud }))
            {
                WorldGen.SquareTileFrame(i, j);
                NetMessage.SendTileSquare(-1, i - 1, j - 1, 3, 3, TileChangeType.None);
            }
        }
    }

    public static bool Chlorophyte(int i, int j)
    {
        int checkMax = 40;
        int closeDistance = 35;
        int farDistance = 85;

        if (j < Main.rockLayer)
        {
            checkMax /= 2;
            closeDistance = (int)(closeDistance * 1.5);
            farDistance = (int)(farDistance * 1.5);
        }

        int count = 0;

        for (int k = i - closeDistance; k < i + closeDistance; k++)
            for (int l = j - closeDistance; l < j + closeDistance; l++)
                if (WorldGen.InWorld(k, l) && Main.tile[k, l].HasTile && Main.tile[k, l].TileType == TileID.Chlorophyte)
                    count++;

        if (count > checkMax)
            return false;

        count = 0;

        for (int m = i - farDistance; m < i + farDistance; m++)
            for (int n = j - farDistance; n < j + farDistance; n++)
                if (WorldGen.InWorld(m, n) && Main.tile[m, n].HasTile && Main.tile[m, n].TileType == TileID.Chlorophyte)
                    count++;

        int wideCheckMax = 130 / ((j < Main.rockLayer) ? 2 : 1);

        if (count > wideCheckMax)
            return false;
        return true;
    }
}
