using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.TileEntities.Verdant;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Tiles.Global;

internal class CoinGlobal : GlobalTile
{
    public override void NearbyEffects(int i, int j, int type, bool closer)
    {
        Tile tile = Main.tile[i, j];

        if (tile.HasTile && tile.TileType == TileID.GoldCoinPile)
        {
            int count = 0;

            for (int k = 0; k <= 1; ++k)
                for (int l = 0; l <= 1; ++l)
                    if (TileHelper.ActiveType(i + k, j + l, TileID.GoldCoinPile))
                        count++;

            var grasses = VerdantGrassLeaves.VerdantGrassTypes.ToArray();
            bool canPlace = TileHelper.ActiveType(i, j + 2, grasses) && TileHelper.ActiveType(i + 1, j + 2, grasses);

            if (count >= 4 && canPlace)
            {
                for (int k = 0; k <= 1; ++k)
                    for (int l = 0; l <= 1; ++l)
                        WorldGen.KillTile(i + k, j + l, false, false, true);

                WorldGen.PlaceTile(i, j, ModContent.TileType<MarigoldTile>());
                ModContent.GetInstance<MarigoldTE>().Place(i, j);
            }
        }
    }
}
