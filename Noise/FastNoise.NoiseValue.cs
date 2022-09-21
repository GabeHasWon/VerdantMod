//Based on https://github.com/Auburns/FastNoise_CSharp/blob/master/FastNoise.cs under MIT License
//Copyright(c) 2017 Jordan Peck

using System;

namespace Verdant.Noise
{
    public partial class FastNoise
    {
        public float GetValue(float x, float y) => SingleValue(Seed, x * Frequency, y * Frequency);
        public float GetValue(float x, float y, float z) => SingleValue(Seed, x * Frequency, y * Frequency, z * Frequency);

        public float GetValueFractal(float x, float y)
        {
            x *= Frequency;
            y *= Frequency;

            switch (FractalType)
            {
                case FractalTypes.FBM:
                    return SingleValueFractalFBM(x, y);
                case FractalTypes.Billow:
                    return SingleValueFractalBillow(x, y);
                case FractalTypes.RigidMulti:
                    return SingleValueFractalRigidMulti(x, y);
                default:
                    return 0;
            };
        }

        public float GetValueFractal(float x, float y, float z)
        {
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;

            switch (FractalType)
            {
                case FractalTypes.FBM:
                    return SingleValueFractalFBM(x, y, z);
                case FractalTypes.Billow:
                    return SingleValueFractalBillow(x, y, z);
                case FractalTypes.RigidMulti:
                    return SingleValueFractalRigidMulti(x, y, z);
                default:
                    return 0;
            };
        }

        private float SingleValue(int seed, float x, float y)
        {
            int x0 = FastFloor(x);
            int y0 = FastFloor(y);
            int x1 = x0 + 1;
            int y1 = y0 + 1;

            float xs, ys;
            switch (InterpolationMethod)
            {
                default:
                case Interp.Linear:
                    xs = x - x0;
                    ys = y - y0;
                    break;
                case Interp.Hermite:
                    xs = InterpHermiteFunc(x - x0);
                    ys = InterpHermiteFunc(y - y0);
                    break;
                case Interp.Quintic:
                    xs = InterpQuinticFunc(x - x0);
                    ys = InterpQuinticFunc(y - y0);
                    break;
            }

            float xf0 = Lerp(ValCoord2D(seed, x0, y0), ValCoord2D(seed, x1, y0), xs);
            float xf1 = Lerp(ValCoord2D(seed, x0, y1), ValCoord2D(seed, x1, y1), xs);

            return Lerp(xf0, xf1, ys);
        }

        private float SingleValue(int seed, float x, float y, float z)
        {
            int x0 = FastFloor(x);
            int y0 = FastFloor(y);
            int z0 = FastFloor(z);
            int x1 = x0 + 1;
            int y1 = y0 + 1;
            int z1 = z0 + 1;

            float xs, ys, zs;
            switch (InterpolationMethod)
            {
                default:
                case Interp.Linear:
                    xs = x - x0;
                    ys = y - y0;
                    zs = z - z0;
                    break;
                case Interp.Hermite:
                    xs = InterpHermiteFunc(x - x0);
                    ys = InterpHermiteFunc(y - y0);
                    zs = InterpHermiteFunc(z - z0);
                    break;
                case Interp.Quintic:
                    xs = InterpQuinticFunc(x - x0);
                    ys = InterpQuinticFunc(y - y0);
                    zs = InterpQuinticFunc(z - z0);
                    break;
            }

            float xf00 = Lerp(ValCoord3D(seed, x0, y0, z0), ValCoord3D(seed, x1, y0, z0), xs);
            float xf10 = Lerp(ValCoord3D(seed, x0, y1, z0), ValCoord3D(seed, x1, y1, z0), xs);
            float xf01 = Lerp(ValCoord3D(seed, x0, y0, z1), ValCoord3D(seed, x1, y0, z1), xs);
            float xf11 = Lerp(ValCoord3D(seed, x0, y1, z1), ValCoord3D(seed, x1, y1, z1), xs);

            float yf0 = Lerp(xf00, xf10, ys);
            float yf1 = Lerp(xf01, xf11, ys);

            return Lerp(yf0, yf1, zs);
        }

        private float SingleValueFractalBillow(float x, float y)
        {
            int seed = Seed;
            float sum = Math.Abs(SingleValue(seed, x, y)) * 2 - 1;
            float amp = 1;

            for (int i = 1; i < octaves; i++)
            {
                x *= FractalLacunarity;
                y *= FractalLacunarity;
                amp *= gain;
                sum += (Math.Abs(SingleValue(++seed, x, y)) * 2 - 1) * amp;
            }

            return sum * fractalBounding;
        }

        private float SingleValueFractalBillow(float x, float y, float z)
        {
            int seed = Seed;
            float sum = Math.Abs(SingleValue(seed, x, y, z)) * 2 - 1;
            float amp = 1;

            for (int i = 1; i < octaves; i++)
            {
                x *= FractalLacunarity;
                y *= FractalLacunarity;
                z *= FractalLacunarity;

                amp *= gain;
                sum += (Math.Abs(SingleValue(++seed, x, y, z)) * 2 - 1) * amp;
            }

            return sum * fractalBounding;
        }

        private float SingleValueFractalFBM(float x, float y)
        {
            int seed = Seed;
            float sum = SingleValue(seed, x, y);
            float amp = 1;

            for (int i = 1; i < octaves; i++)
            {
                x *= FractalLacunarity;
                y *= FractalLacunarity;

                amp *= gain;
                sum += SingleValue(++seed, x, y) * amp;
            }

            return sum * fractalBounding;
        }

        private float SingleValueFractalFBM(float x, float y, float z)
        {
            int seed = Seed;
            float sum = SingleValue(seed, x, y, z);
            float amp = 1;

            for (int i = 1; i < octaves; i++)
            {
                x *= FractalLacunarity;
                y *= FractalLacunarity;
                z *= FractalLacunarity;

                amp *= gain;
                sum += SingleValue(++seed, x, y, z) * amp;
            }

            return sum * fractalBounding;
        }

        private float SingleValueFractalRigidMulti(float x, float y)
        {
            int seed = Seed;
            float sum = 1 - Math.Abs(SingleValue(seed, x, y));
            float amp = 1;

            for (int i = 1; i < octaves; i++)
            {
                x *= FractalLacunarity;
                y *= FractalLacunarity;

                amp *= gain;
                sum -= (1 - Math.Abs(SingleValue(++seed, x, y))) * amp;
            }

            return sum;
        }

        private float SingleValueFractalRigidMulti(float x, float y, float z)
        {
            int seed = Seed;
            float sum = 1 - Math.Abs(SingleValue(seed, x, y, z));
            float amp = 1;

            for (int i = 1; i < octaves; i++)
            {
                x *= FractalLacunarity;
                y *= FractalLacunarity;
                z *= FractalLacunarity;

                amp *= gain;
                sum -= (1 - Math.Abs(SingleValue(++seed, x, y, z))) * amp;
            }

            return sum;
        }
    }
}
