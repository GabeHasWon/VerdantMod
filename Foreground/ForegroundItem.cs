using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Verdant.Foreground
{
    public class ForegroundItem
    {
        public Vector2 position = new Vector2(0, 0);
        internal Vector2 drawPosition = new Vector2();
        public Vector2 velocity = new Vector2(0, 0);
        public float scale = 1f;
        public Rectangle source = new Rectangle();
        public Color drawColor = Color.White;

        internal bool drawLighted = true;

        public bool killMe = false; //love this

        public virtual bool SaveMe => false;

        public Vector2 Center => position + (source.Size() / 2f);

        public readonly Texture2D tex;

        public ForegroundItem(Vector2 pos, Vector2 vel, float sc, string path)
        {
            position = pos;
            velocity = vel;
            tex = ModContent.GetTexture($"Verdant/Foreground/{path}");
            scale = sc;

            source = new Rectangle(0, 0, tex.Width, tex.Height);
        }

        public virtual void Update()
        {
            position += velocity;
        }

        public virtual void Draw()
        {
            Main.spriteBatch.Draw(tex, drawPosition - Main.screenPosition, source, drawColor, 0f, tex.Size() / 2, scale, SpriteEffects.None, 0f);
        }

        /// <summary>Called when saving this ForegroundItem.</summary>
        /// <returns>Value(s) to be saved.</returns>
        public virtual TagCompound Save()
        {
            return null;
        }

        /// <summary>Called on world loading.</summary>
        /// <param name="info"></param>
        public virtual void Load(TagCompound info)
        {
        }

        public override string ToString() => $"{GetType().Name} at {position}\nSIZE: {scale}, SAVE: {SaveMe}, LIGHTED: {drawLighted}";
    }
}
