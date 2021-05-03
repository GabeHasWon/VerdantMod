using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Verdant.Foreground.Tiled
{
    public class TiledForegroundItem : ForegroundItem
    {
        int width = 0;
        int height = 0;

        public TiledForegroundItem(Point tilePosition, string path, Point size, bool copyTileFrame, bool lighted = true) : base(tilePosition.ToVector2() * 16, new Vector2(0, 0), new Vector2(1), path)
        {
            width = size.X;
            height = size.Y;

            if (copyTileFrame)
                CopyTileFrame();

            drawLighted = lighted;
        }

        public override void Update()
        {
            Tile t = Framing.GetTileSafely((int)(position.X / 16), (int)(position.Y / 16));
            if (!t.active())
                killMe = true;
        }

        public override void Draw()
        {
            Texture2D tex = ForegroundManager.GetTexture(texPath);
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    Rectangle rect = new Rectangle(source.X + (18 * i), source.Y + (18 * j), 16, 16);
                    Color col = drawLighted ? Lighting.GetColor((int)(position.X / 16), (int)(position.Y / 16)) : Color.White;
                    Main.spriteBatch.Draw(tex, position - Main.screenPosition + (new Vector2(i, j) * 16), rect, col, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
            }
        }

        public void CopyTileFrame()
        {
            Tile t = Framing.GetTileSafely(position);
            source = new Rectangle(t.frameX, t.frameY, 16, 16);
        }
    }
}