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

        public Vector2 RealPosition { get; private set; }

        internal Vector2 Center { get => position + (tex.Bounds.Size() / 2); }

        public BaseBGItem()
        {
        }

        public BaseBGItem(Vector2 initPos, float sc, Point size)
        {
            position = initPos;
            scale = sc;

            source = new Rectangle(0, 0, size.X, size.Y);

            RealPosition = position;
        }

        public BaseBGItem(Texture2D t, Vector2 initPos, float sc)
        {
            tex = t;
            position = initPos;
            scale = sc;

            source = new Rectangle(0, 0, t.Width, t.Height);
            RealPosition = position;
        }

        internal virtual void Behaviour()
        {
            position += velocity;
        }

        internal virtual void Draw(Vector2 off)
        {
            RealPosition = position + off;
            Main.spriteBatch.Draw(tex, RealPosition - Main.screenPosition, source, drawColor, rotation, tex.Bounds.Center.ToVector2(), scale, SpriteEffects.None, 0f);
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