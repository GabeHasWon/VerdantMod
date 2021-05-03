using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;
using Verdant.Foreground.Tiled;

namespace Verdant.Foreground
{
    public static class ForegroundManager
    {
        private static List<ForegroundItem> items = new List<ForegroundItem>();

        public static List<ForegroundItem> loadClasses = new List<ForegroundItem>();

        public static void Run()
        {
            List<ForegroundItem> removals = new List<ForegroundItem>();

            //Rectangle screen = new Rectangle((int)Main.screenPosition.X - Main.offScreenRange, (int)Main.screenPosition.Y - Main.offScreenRange, Main.screenWidth + Main.offScreenRange, Main.screenHeight + Main.offScreenRange);

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

        public static void Load(IList<ForegroundData> info)
        {
            loadClasses.Add(new VerdantBush(Point.Zero));

            foreach (var item in info)
            {
                ForegroundItem val = loadClasses[item.type];
                val.position = item.position;
                AddItem(val);
            }
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

        internal static List<ForegroundData> Save()
        {
            List<ForegroundData> data = new List<ForegroundData>();
            foreach (var item in items)
            {
                if (item.saveMe)
                {
                }
            }
            return data;
        }
    }
}
