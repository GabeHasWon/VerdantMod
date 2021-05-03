using Microsoft.Xna.Framework;
using System;

namespace Verdant.Foreground
{
    public struct ForegroundData
    {
        public int type;
        public Vector2 position;

        public ForegroundData(int t, Vector2 pos)
        {
            type = t;
            position = pos;
        }
    }
}
