using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;

namespace Verdant.Foreground
{
    public static class ForegroundManager
    {
        private static readonly List<ForegroundItem> items = new List<ForegroundItem>();

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

        //public static void Load(TagCompound info)
        //{
        //}

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

        internal static TagCompound Save()
        {
            TagCompound compound = new TagCompound();
            foreach (var item in items)
            {
                if (item.SaveMe)
                {
                    var value = item.Save();
                    if (value == null)
                        continue;

                    compound.Add("fgInfo", value);
                }
            }
            return compound;
        }
    }
}
