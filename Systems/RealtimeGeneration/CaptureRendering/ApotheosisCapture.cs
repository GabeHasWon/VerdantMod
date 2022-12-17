using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Verdant.Systems.RealtimeGeneration.CaptureRendering
{
    internal class ApotheosisCapture : CaptureData
    {
        private int _timer = 0;

        public ApotheosisCapture(string name) : base(name) { }

        internal override void Update()
        {
            _timer++;
        }

        internal override void DrawTarget(RenderTarget2D overlay)
        {
            Main.spriteBatch.Draw(overlay, Main.MouseScreen - overlay.Size() / 2, Color.White * (1 - (_timer / 400f)));
        }
    }
}
