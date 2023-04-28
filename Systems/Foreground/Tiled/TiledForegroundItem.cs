using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Verdant.Systems.Foreground.Tiled
{
    public class TiledForegroundItem : ForegroundItem
    {
        int width = 0;
        int height = 0;

        public TiledForegroundItem(Point tilePosition, string path, Point size, bool copyTileFrame, bool lighted = true) : base(tilePosition.ToWorldCoordinates(0, 0), new Vector2(0, 0), 1f, path)
        {
            width = size.X;
            height = size.Y;

            if (copyTileFrame)
                CopyTileFrame();

            drawLighted = lighted;
        }

        public override void Update()
        {
            CheckAnchor();
        }

        protected virtual void CheckAnchor()
        {
            Tile t = Framing.GetTileSafely(position.ToTileCoordinates());

            if (!t.HasTile)
                killMe = true;
        }

        public override void Draw()
        {
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    Rectangle rect = new(source.X + (18 * i), source.Y + (18 * j), 16, 16);
                    Color col = drawLighted ? Lighting.GetColor((int)(position.X / 16), (int)(position.Y / 16)) : Color.White;
                    Main.spriteBatch.Draw(Texture.Value, position - Main.screenPosition + (new Vector2(i, j) * 16), rect, col, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
            }
        }

        public void CopyTileFrame()
        {
            Tile t = Framing.GetTileSafely(position);
            source = new Rectangle(t.TileFrameX, t.TileFrameY, 16, 16);
        }
    }
}