using Microsoft.Xna.Framework;

namespace Verdant.Systems.ScreenText.Animations
{
    internal class FadeAnimation : IScreenTextAnimation
    {
        public void ModifyDraw(float factor, ScreenText self, ref Vector2 position, ref Color drawColor, ref Color speakerColor, ref float scale)
        {
            drawColor *= factor + 0.5f;
            speakerColor *= factor + 0.5f;

            if (factor + 0.5f <= 0)
                self.active = false;
        }
    }
}
