using Microsoft.Xna.Framework;

namespace Verdant.Systems.RealtimeGeneration.CaptureRendering
{
    internal readonly struct CaptureData
    {
        public readonly Rectangle Area;

        public CaptureData(Rectangle area)
        {
            Area = area;
        }
    }
}
