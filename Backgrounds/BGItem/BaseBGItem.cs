using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader.IO;

namespace Verdant.Backgrounds.BGItem
{
    public class BaseBGItem
    {
        internal Texture2D tex;
        internal Vector2 position = new Vector2();
        internal Vector2 velocity = new Vector2();
        internal float rotation = 0f;
        internal Vector2 scale = Vector2.One;
        /// <summary>If true, this background item will be removed next frame.</summary>
        internal bool killMe = false;
        /// <summary>Adjusts how quickly or slowly it moves according to parallax, and is used for layering.</summary>
        internal float parallax = 1f;
        /// <summary>The colour to draw this like.</summary>
        internal Color drawColor = Color.White;
        /// <summary>SourceRectangle for different frames.</summary>
        internal Rectangle source = new Rectangle(0, 0, 0, 0);

        /// <summary>If true, this background item will be saved and loaded, as per <see cref="Save()"/> and <see cref="Load(TagCompound)"/>.</summary>
        public virtual bool SaveMe => false;
        /// <summary>Used for draw position, so that stuff that is offscreen does not need to be drawn. Might not work, needs tweaking.</summary>
        public Vector2 DrawPosition { get; protected set; }

        public float Scale
        {
            get => scale.Length();
            set => scale = new Vector2(value);
        }

        /// <summary>Center of the background item.</summary>
        internal Vector2 Center => position + (source.Size() / 2);

        /// <summary>Default with only a save/don't save value.</summary>
        public BaseBGItem()
        {
        }

        /// <summary>Creates a BGItem with a position, scale and frame size.</summary>
        /// <param name="initPos">Position this BGItem is spawned at.</param>
        /// <param name="sc">Scale this BGItem is spawned with.</param>
        /// <param name="size">Frame size.</param>
        public BaseBGItem(Vector2 initPos, Vector2 sc, Point size)
        {
            position = initPos;
            scale = sc;

            source = new Rectangle(0, 0, size.X, size.Y);

            DrawPosition = position;
        }

        /// <summary>Creates a BGItem with a texture, position and scale.</summary>
        /// <param name="t">Texture this BGItem uses.</param>
        /// <param name="initPos">Position this BGItem is spawned at.</param>
        /// <param name="sc">Scale this BGItem is spawned with.</param>
        public BaseBGItem(Texture2D t, Vector2 initPos, Vector2 sc)
        {
            tex = t;
            position = initPos;
            scale = sc;

            source = new Rectangle(0, 0, t.Width, t.Height);
            DrawPosition = position;
        }

        /// <summary>Equivalent to Update(). Adds velocity to position by default.</summary>
        internal virtual void Behaviour() => position += velocity;

        /// <summary>Draws the BGItem.</summary>
        /// <param name="off">Offset for drawing.</param>
        internal virtual void Draw(Vector2 off)
        {
            DrawPosition = GetParallax();
            Main.spriteBatch.Draw(tex, DrawPosition - Main.screenPosition + off, source, drawColor, rotation, tex.Bounds.Center.ToVector2(), scale, SpriteEffects.None, 0f);
        }

        /// <summary>Weird hacky thing I did for parallax. Offsets position to look parallaxed.</summary>
        /// <returns>Parallax value.</returns>
        internal Vector2 GetParallax()
        {
            Vector2 pC = Main.screenPosition + (new Vector2(Main.screenWidth, Main.screenHeight) / 2f) + (Vector2.UnitY * Main.LocalPlayer.gfxOffY);
            Vector2 real = Vector2.Lerp(pC, position, parallax);
            return real;
        }

        /// <summary>Saves the current BGItem.</summary>
        /// <returns>Info to save.</returns>
        public virtual TagCompound Save() => null;

        /// <summary>Loads info given a tag.</summary>
        /// <param name="tag">Info to use to load.</param>
        public virtual void Load(TagCompound tag) { }
    }
}