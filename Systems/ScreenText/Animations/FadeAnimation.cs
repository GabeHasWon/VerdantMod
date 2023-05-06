using Microsoft.Xna.Framework;

namespace Verdant.Systems.ScreenText.Animations;

internal class FadeAnimation : IScreenTextAnimation
{
    public void ModifyDraw(float factor, ScreenText self, ref Vector2 position, ref Color lineColor, ref Color speakerColor, ref float scale)
    {
        lineColor *= factor;
        speakerColor *= factor;
        lineColor *= factor;

        if (factor <= 0)
            self.active = false;
    }
}
