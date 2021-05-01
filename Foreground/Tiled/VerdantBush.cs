using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verdant.Foreground.Tiled
{
    public class VerdantBush : TiledForegroundItem
    {
        static VerdantBush() //jesus CHRIST a static constructor
        {

        }

        public VerdantBush(Point pos) : base(pos, "VerdantBushes", new Point(1, 2), true, true)
        {
        }
    }
}
