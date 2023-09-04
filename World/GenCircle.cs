using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Utilities;

namespace Verdant.World
{
    /// <summary>Simple struct for genning the base shape of the Verdant.</summary>
    internal struct GenCircle
    {
        public const float MaxDitherDistance = 8f;

        public int radius;
        public Point16 center;
        public List<Point16> tiles;

        public GenCircle(int radius, Point16 center)
        {
            this.radius = radius;
            this.center = center;

            tiles = new List<Point16>();
        }

        public override readonly string ToString() => radius + " + " + center;

        public readonly void FindTiles(bool horiSort = true, bool biomeNoise = true, UnifiedRandom random = null)
        {
            random ??= WorldGen.genRand;

            if (horiSort)
            {
                for (int i = -radius; i < radius; ++i)
                    for (int j = -radius; j < radius; ++j)
                        SingleCheck(i, j, biomeNoise, random);
            }
            else
            {
                for (int j = -radius; j < radius; ++j)
                    for (int i = -radius; i < radius; ++i)
                        SingleCheck(i, j, biomeNoise, random);
            }
        }

        private readonly void SingleCheck(int i, int j, bool biomeNoise, UnifiedRandom random = null)
        {
            random ??= WorldGen.genRand;

            Point16 nPos = new(center.X + i, center.Y + j);
            float dist = Vector2.Distance(nPos.ToVector2(), center.ToVector2());

            if (dist < radius)
            {
                if (radius - dist < MaxDitherDistance)
                {
                    if (biomeNoise)
                    {
                        float chance = (radius - dist) / MaxDitherDistance;

                        if (random.NextFloat() <= chance && Main.tile[nPos.X, nPos.Y].HasTile)
                            tiles.Add(nPos);
                    }
                }
                else
                    tiles.Add(nPos);
            }
        }
    }
}
