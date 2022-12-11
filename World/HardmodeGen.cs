using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Decor;

namespace Verdant.World
{
    internal class HardmodeGen : ModSystem
    {
        public override void ModifyHardmodeTasks(List<GenPass> list)
        {
            list.Add(new PassLegacy("Verdant Replacements", HardmodeTasks));
        }

        public void HardmodeTasks(GenerationProgress p, GameConfiguration config)
        {
            for (int x = 40; x <= Main.maxTilesX - 40; ++x)
            {
                for (int y = 40; y <= Main.maxTilesY - 40; ++y)
                {
                    Tile tile = Main.tile[x, y];

                    if (!tile.HasTile)
                        continue;

                    Replace(x, y, ModContent.TileType<VerdantVine>(), ModContent.TileType<LightbulbVine>());
                    Replace(x, y, ModContent.TileType<Apotheosis>(), ModContent.TileType<HardmodeApotheosis>());
                }
            }
        }

        private static void Replace(int x, int y, int replace, int newType)
        {
            Tile tile = Main.tile[x, y];

            if (tile.TileType == replace)
                tile.TileType = (ushort)newType;
        }
    }
}
