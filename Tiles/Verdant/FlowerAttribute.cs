using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Verdant.Tiles.Verdant
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class FlowerAttribute : Attribute
    {
        public Vector2[] Flowers;

        public FlowerAttribute(params string[] offsets)
        {
            List<Vector2> vec2 = new List<Vector2>();
            foreach (var item in offsets)
            {
                string[] split = item.Split(" ");

                if (split.Length != 2)
                    throw new ArgumentException("Invalid Vector2 string length");

                if (!float.TryParse(split[0], out float x))
                    throw new ArgumentException("X component isn't a valid float");
                if (!float.TryParse(split[1], out float y))
                    throw new ArgumentException("Y component isn't a valid float");

                vec2.Add(new Vector2(x, y));
            }
            Flowers = vec2.ToArray();
        }
    }
}
