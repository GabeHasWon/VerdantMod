using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;

namespace Verdant.Foreground
{
    public static class ForegroundManager
    {
        private static List<ForegroundItem> items = new List<ForegroundItem>();

        public static Dictionary<int, ForegroundItem> loadClasses = new Dictionary<int, ForegroundItem>();

        public static void Run()
        {
            List<ForegroundItem> removals = new List<ForegroundItem>();

            foreach (var val in items)
            {
                val.Update();
                val.Draw();
                if (val.killMe)
                    removals.Add(val);
            }

            foreach (var item in removals)
                items.Remove(item);
        }

        public static void Unload()
        {
            items.Clear();
        }

        public static void AddItem(ForegroundItem item)
        {
            items.Add(item);
        }

        /// <summary>Shorthand for ModContent.GetTexture("Verdant/Foreground/Textures/" + name).</summary>
        /// <param name="name">Name of the requested texture.</param>
        public static Texture2D GetTexture(string name) => VerdantMod.Instance.GetTexture("Foreground/Textures/" + name);

        internal static List<ForegroundItem> Save()
        {
            //List<ForegroundData> data = new List<ForegroundData>();
            //foreach (var item in items)
            //{
            //    if (item.saveMe)
            //        data.Add(new ForegroundData(item.GetType(), item.position));
            //}
            return items;
        }
    }
}
