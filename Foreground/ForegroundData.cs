using Microsoft.Xna.Framework;
using System;

namespace Verdant.Foreground
{
    public struct ForegroundData
    {
        public Type type;
        public Vector2 position;

        public ForegroundData(Type t, Vector2 pos)
        {
            type = t;
            position = pos;
        }
    }
}
