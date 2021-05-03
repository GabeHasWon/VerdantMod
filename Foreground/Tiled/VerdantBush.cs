using Microsoft.Xna.Framework;

namespace Verdant.Foreground.Tiled
{
    public class VerdantBush : TiledForegroundItem
    {
        public VerdantBush(Point pos) : base(pos, "VerdantBushes", new Point(1, 2), true, true)
        {
        }
    }
}