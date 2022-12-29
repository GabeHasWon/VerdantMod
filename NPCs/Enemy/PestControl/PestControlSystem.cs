using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Verdant.NPCs.Enemy.PestControl
{
    internal class PestControlSystem : ModSystem
    {
        public bool PestControl = false;
        public Point PestControlCenter = Point.Zero;

        public override void PreUpdateNPCs()
        {
            base.PreUpdateNPCs();
        }
    }
}
