using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Verdant.Systems.RealtimeGeneration.CaptureRendering
{
    internal class ApotheosisCapture : CaptureData
    {
        private readonly int MaxTime = 0;

        private int _timer = 0;

        public ApotheosisCapture(string name, int maxTime) : base(name)
        {
            MaxTime = maxTime;
            Run = true;
        }

        internal override void Update()
        {
            _timer++;

            if (_timer > MaxTime)
            {
                RealtimeGen.ReplaceStructure("Testing");

                Action.finished = true;
            }
        }

        internal override void DrawTarget(Texture2D overlay)
        {
            Main.spriteBatch.Draw(overlay, Main.MouseScreen - overlay.Size() / 2, Color.White * (1 - (_timer / (float)MaxTime)));
        }
    }
}
