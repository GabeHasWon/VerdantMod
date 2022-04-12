using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Verdant.Drawing
{
    public static class DrawHelper
    {
        public static void DrawWorld(Texture2D tex, Vector2 position, Rectangle? rect = null, Color? col = null, float rot = 0f, Vector2? orig = null, Vector2? scale = null, SpriteEffects effects = SpriteEffects.None)
        {
            Point tPos = position.ToTileCoordinates();
            Main.spriteBatch.Draw(tex, position - Main.screenPosition, rect, col ?? Lighting.GetColor(tPos.X, tPos.Y), rot, orig ?? Vector2.Zero, scale ?? Vector2.One, effects, 0f);
        }
    }
}
