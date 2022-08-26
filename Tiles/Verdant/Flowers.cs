using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant
{
    internal class Flowers
    {
        public static IReadOnlyCollection<int> FlowerIDs;

        public static void Load(Mod mod)
        {
            var types = mod.GetType().Assembly.GetTypes().Where(x => !x.IsAbstract && Attribute.IsDefined(x, typeof(FlowerAttribute)) && typeof(ModTile).IsAssignableFrom(x));
            List<int> ids = new List<int>();

            foreach (var type in types)
            {
                int id = mod.Find<ModTile>(type.Name).Type;
                ids.Add(id);
            }

            FlowerIDs = ids.AsReadOnly();

        }

        public static void 
    }
}
