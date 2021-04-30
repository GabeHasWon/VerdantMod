using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verdant.World
{
    [Obsolete("Legacy code prior to Cubic-Interpolation addition - use that instead.", true)]
    class LegacyGenWeighted
    {
        public struct GenLineWeighted
        {
            public Vector2 leftEnd;
            public Vector2 rightEnd;
            public int pointCount;
            public float pointDist;
            public int steps;

            private Vector2 Middle
            {
                get => (rightEnd + leftEnd) / 2f;
            }

            public SpecialVector2[] Segments { get; private set; }

            /// <summary>
            /// Generates and simulates a line w/ weight per segment.
            /// </summary>
            /// <param name="l">Left anchor</param>
            /// <param name="r">Right anchor</param>
            /// <param name="c">Number of segments</param>
            /// <param name="release">How taut the line is</param>
            /// <param name="s">Steps. Defaults to c/2.</param>
            [Obsolete("Legacy code prior to Cubic-Interpolation addition - use that instead.", true)]
            public GenLineWeighted(Vector2 l, Vector2 r, int c, float release = 0.75f, int s = -1)
            {
                leftEnd = l;
                rightEnd = r;
                pointCount = c;
                steps = s;
                if (s == -1) steps = c / 2;

                // pointDist = ((r.X - l.X) / c) * release;
                pointDist = Vector2.Distance(l, r) / 2f;

                Segments = new SpecialVector2[c];
                for (int i = 0; i < c; ++i)
                {
                    float x = leftEnd.X + (((rightEnd.X - leftEnd.X) / c) * i); //Initial segmentation
                    float y = leftEnd.Y + (((rightEnd.Y - leftEnd.Y) / c) * i);

                    Segments[i] = new SpecialVector2(x, y);
                }
                Segments[0].final = true;
                Segments[c - 1].final = true;
                SimulateUntilEnd();
            }

            private void SimulateUntilEnd()
            {
                for (int j = 0; j < steps; ++j)
                {
                    Vector2 l = leftEnd;
                    Vector2 r = rightEnd;
                    SpecialVector2[] orderedPoints = Segments.OrderByDescending(x => Vector2.Distance(new Vector2(x.X, x.Y), l) >= Vector2.Distance(new Vector2(x.X, x.Y), r)
                        ? Vector2.Distance(new Vector2(x.X, x.Y), l) : Vector2.Distance(new Vector2(x.X, x.Y), r)).ToArray();
                    for (int i = 0; i < orderedPoints.Length; ++i)
                        Simulate(i);
                    for (int i = 1; i < Segments.Length - 1; ++i)
                        Segments[i].final = false;
                }
            }

            private void Simulate(int index)
            {
                while (!Segments[index].final)
                {
                    Segments[index].Y += 0.75f;
                    //Vector2.Distance(new Vector2(Segments[index].X, Segments[index].Y), new Vector2(Segments[index - 1].X, Segments[index - 1].Y)) +
                    //Vector2.Distance(new Vector2(Segments[index].X, Segments[index].Y), new Vector2(Segments[index + 1].X, Segments[index + 1].Y)) > pointDist * 2f ||
                    //Framing.GetTileSafely((int)Segments[index].X, (int)Segments[index].Y).active()
                    if (Vector2.Distance(Middle, Segments[index].ToVector2()) > pointDist) //||
                        Segments[index].final = true;
                }
            }
        }

        public struct SpecialVector2
        {
            public float X;
            public float Y;
            public bool final;

            public SpecialVector2(float x, float y)
            {
                X = x;
                Y = y;
                final = false;
            }

            public Vector2 ToVector2() => new Vector2(X, Y);
            public override string ToString() => $"({X}, {Y}), {final}";
        }
    }
}
