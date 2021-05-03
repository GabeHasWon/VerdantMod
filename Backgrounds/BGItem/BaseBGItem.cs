using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace Verdant.Backgrounds.BGItem
{
    public class BaseBGItem
    {
        internal Texture2D tex;
        internal Vector2 position = new Vector2();
        internal Vector2 velocity = new Vector2();
        internal float rotation = 0f;
        internal float scale = 1f;
        internal bool killMe = false;
        internal float parallax = 1f;
        internal float parallaxScale = 1f;
        internal Color drawColor = Color.White;
        internal Rectangle source = new Rectangle(0, 0, 0, 0);

        public List<BaseBGItem> bgItems = new List<BaseBGItem>();

        internal Vector2 Center { get => position + (tex.Bounds.Size() / 2); }

        public BaseBGItem()
        {
        }

        public BaseBGItem(Vector2 initPos, float sc, Point size)
        {
            position = initPos;
            scale = sc;

            source = new Rectangle(0, 0, size.X, size.Y);
        }

        public BaseBGItem(Texture2D t, Vector2 initPos, float sc)
        {
            tex = t;
            position = initPos;
            scale = sc;

            source = new Rectangle(0, 0, t.Width, t.Height);
        }

        public void RunAll(bool doUpdate)
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
                if (screen.Contains(item.position.ToPoint()))
                {
                    if (item.position.Y / 16f < Main.worldSurface)
                        item.Draw(off);
                }
            }

            foreach (var item in removeList)
                bgItems.Remove(item);
        }

        internal virtual void Behaviour()
        {
            position += velocity;
        }

        internal virtual void Draw(Vector2 off)
        {
            Main.spriteBatch.Draw(tex, position + off - Main.screenPosition, source, drawColor, rotation, tex.Bounds.Center.ToVector2(), scale, SpriteEffects.None, 0f);
        }

        internal Vector2 GetParallax()
        {
            Vector2 pC = Main.LocalPlayer.Center + (Vector2.UnitY * Main.LocalPlayer.gfxOffY);
            Vector2 offset = ((pC - position) * parallax);
            return offset;
        }

        internal void BaseParallax(float mul)
        {
            parallax = (0.8f - scale) * parallaxScale * mul;
            if (parallax > 1) parallax = 1f;
        }
        internal void BaseParallax() => BaseParallax(1f);
    }
}