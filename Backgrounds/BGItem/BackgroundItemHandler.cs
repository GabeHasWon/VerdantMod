using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;

namespace Verdant.Backgrounds.BGItem
{
    public class BackgroundItemHandler
    {
        private static List<BaseBGItem> bgItems;

        public static bool Loaded { get; private set; }

        public static void Load()
        {
            bgItems = new List<BaseBGItem>();
            Loaded = true;
        }

        public static void Unload()
        {
            bgItems = null;
            Loaded = false;
        }

        public static void AddItem(BaseBGItem item) => bgItems.Add(item);

        public static void RunAll(bool doUpdate)
        {
            List<BaseBGItem> removeList = new List<BaseBGItem>();
            var order = bgItems.OrderBy(x => x.scale);

            Rectangle screen = new Rectangle((int)Main.screenPosition.X - Main.offScreenRange, (int)Main.screenPosition.Y - Main.offScreenRange, Main.screenWidth + Main.offScreenRange, Main.screenHeight + Main.offScreenRange);

            foreach (var item in order)
            {
                if (item.killMe)
                {
                    removeList.Add(item);
                    continue;
                }
                if (doUpdate)
                    item.Behaviour();
                Vector2 off = Lighting.lightMode > 1 ? Vector2.Zero : Vector2.One;
                if (screen.Contains((item.RealPosition).ToPoint()))
                {
                    if (item.position.Y / 16f < Main.worldSurface)
                        item.Draw(off);
                }
            }

            foreach (var item in removeList)
                bgItems.Remove(item);
        }
    }
}
