using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Verdant.Foreground
{
    public class ForegroundItem
    {
        public Vector2 position = new Vector2(0, 0);
        public Vector2 velocity = new Vector2(0, 0);
        public Vector2 scale = new Vector2();
        public Rectangle source = new Rectangle();

        internal bool drawLighted = true;

        public bool killMe = false; //love this
        public bool saveMe = true;

        public readonly string texPath = "None";

        public ForegroundItem(Vector2 pos, Vector2 vel, Vector2 sc, string path)
        {
            position = pos;
            velocity = vel;
            texPath = path;
            scale = sc;

            ForegroundManager.AddItem(this);
        }

        public virtual void Update()
        {
            position += velocity;
        }

        public virtual void Draw()
        {
            Texture2D tex = ForegroundManager.GetTexture(texPath);
            Main.spriteBatch.Draw(tex, position - Main.screenPosition, null, Color.White, 0f, tex.Size() / 2, scale, SpriteEffects.None, 0f);
        }

        public override string ToString() => $"{GetType().Name} at {position} using {texPath}\nSIZE: {scale}, SAVE: {saveMe}, LIGHTED: {drawLighted}";
    }
}
