using Microsoft.Xna.Framework;
using Terraria;

namespace Verdant
{
    public static class PlayerHelper
    {
        public static Vector2 PlayerDrawPositionOffset(Player p, Vector2 offset)
        {
            return new Vector2(p.position.X - Main.screenPosition.X, p.position.Y - Main.screenPosition.Y + p.gfxOffY) + new Vector2(Main.offScreenRange, Main.offScreenRange) - new Vector2(182, 142) + offset;
        }
    }
}
