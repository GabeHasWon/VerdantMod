using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace Verdant.World
{
    /// <summary>Simple struct for genning the base shape of the Verdant.</summary>
    internal struct GenCircle
    {
        public int rad;
        public Point16 pos;
        public List<Point16> tiles;

        public GenCircle(int r, Point16 p)
        {
            rad = r;
            pos = p;

            tiles = new List<Point16>();
        }

        public override string ToString() => rad + " + " + pos;

        public void Gen()
        {
            const float MaxDitherDistance = 8f;

            for (int i = -rad; i < rad; ++i)
            {
                for (int j = -rad; j < rad; ++j)
                {
                    Point16 nPos = new(pos.X + i, pos.Y + j);
                    float dist = Vector2.Distance(nPos.ToVector2(), pos.ToVector2());

                    if (dist < rad)
                    {
                        if (rad - dist < MaxDitherDistance)
                        {
                            float chance = (rad - dist) / MaxDitherDistance;

                            if (WorldGen.genRand.NextFloat() <= chance && Main.tile[nPos.X, nPos.Y].HasTile)
                                tiles.Add(nPos);
                        }
                        else
                            tiles.Add(nPos);
                    }
                }
            }
        }
    }
}
