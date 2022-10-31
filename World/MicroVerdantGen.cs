using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Verdant.Noise;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Walls;
using Verdant.World.RealtimeGeneration;

namespace Verdant.World
{
    internal class MicroVerdantGen
    {
        public static Queue<(Point, TileAction.TileActionDelegate)> MicroVerdant()
        {
            var queue = new Queue<(Point, TileAction.TileActionDelegate)>();

            GenCircle circle = new GenCircle((int)(VerdantGenSystem.WorldSize * 40), Main.MouseWorld.ToTileCoordinates16());
            circle.Gen();

            VerdantSystem.genNoise = new FastNoise(Main.rand.Next(int.MaxValue));
            VerdantSystem.genNoise.Seed = WorldGen._genRandSeed;
            VerdantSystem.genNoise.Frequency = 0.05f;
            VerdantSystem.genNoise.NoiseType = FastNoise.NoiseTypes.CubicFractal; //Sets noise to proper type
            VerdantSystem.genNoise.FractalType = FastNoise.FractalTypes.Billow;

            foreach (var point in circle.tiles)
            {
                float n = VerdantSystem.genNoise.GetNoise(point.X, point.Y);

                TileAction.TileActionDelegate action = (_, _) => { };

                if (n < -0.67f) { }
                else if (n < -0.57f)
                    action += TileAction.PlaceTileAction(ModContent.TileType<VerdantGrassLeaves>(), false); //WorldGen.PlaceTile(point.X, point.Y, TileTypes[0]);
                else
                    action += TileAction.PlaceTileAction(ModContent.TileType<LushSoil>(), false);

                if (n < -0.85f)
                    action += TileAction.KillWall();
                else if (n < -0.52f)
                    action += TileAction.PlaceWall(ModContent.WallType<VerdantLeafWall_Unsafe>());
                else
                    action += TileAction.PlaceWall(ModContent.WallType<LushSoilWall_Unsafe>());

                if (action.GetInvocationList().Length > 1)
                    queue.Enqueue((point.ToPoint(), action));
            }
            return queue;
        }
    }
}
