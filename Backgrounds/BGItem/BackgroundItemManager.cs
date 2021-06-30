using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.IO;

namespace Verdant.Backgrounds.BGItem
{
    public class BackgroundItemManager
    {
        /// <summary>List of items to draw and update.</summary>
        private static List<BaseBGItem> bgItems;
        /// <summary>Organized list of items for layering.</summary>
        private static IOrderedEnumerable<BaseBGItem> organizedItems;

        /// <summary>True when this has been loaded and is usable.</summary>
        public static bool Loaded { get; private set; }

        public static void AddItem(BaseBGItem item) => bgItems.Add(item);

        /// <summary></summary>
        /// <param name="doUpdate"></param>
        public static void Draw()
        {
            int range = Main.offScreenRange * 3;
            Rectangle screen = new Rectangle((int)Main.screenPosition.X - range, (int)Main.screenPosition.Y - range, Main.screenWidth + (range * 2), Main.screenHeight + (range * 2));

            organizedItems = bgItems.OrderBy(x => x.scale); //For proper depth

            foreach (var item in organizedItems) //Draw everything
            {
                Vector2 off = Lighting.lightMode > 1 ? Vector2.Zero : Vector2.One;
                if (screen.Contains((item.DrawPosition).ToPoint()))
                {
                    if (item.position.Y / 16f < Main.worldSurface)
                        item.Draw(off);
                }
            }
        }

        public static void Update()
        {
            List<BaseBGItem> removeList = new List<BaseBGItem>();

            foreach (var item in organizedItems)
            {
                if (item.killMe) //Remove items that need to be removed
                {
                    removeList.Add(item);
                    continue;
                }

                item.Behaviour();
            }

            foreach (var item in removeList)
                bgItems.Remove(item);
        }

        public static List<TagCompound> Save()
        {
            List<TagCompound> tags = new List<TagCompound>(); //Save the stuff
            foreach (var item in bgItems)
            {
                if (item.SaveMe && !item.killMe)
                {
                    TagCompound value = item.Save();
                    if (value != null)
                    {
                        value.Add("Name", item.GetType().FullName); //So I can get the type later
                        tags.Add(value);
                    }
                }
            }

            bgItems = null; //Clear it and unload
            organizedItems = null;
            Loaded = false;
            return tags;
        }

        public static void Load(IList<TagCompound> info)
        {
            bgItems = new List<BaseBGItem>(); //Load
            organizedItems = bgItems.OrderBy(x => x.scale);
            Loaded = true;

            foreach (var item in info)
            {
                string name = item.GetString("Name");
                try
                {
                    Type bgItemType = typeof(BackgroundItemManager).Assembly.GetType(name, true);
                    BaseBGItem bgItem = Activator.CreateInstance(bgItemType) as BaseBGItem;
                    bgItem.Load(item);
                    bgItems.Add(bgItem);
                }
                catch (Exception e)
                {
                    VerdantMod mod = VerdantMod.Instance;
                    mod.Logger.Warn("Failed to load assembly type.\n", e);
                }
            }
        }
    }
}
