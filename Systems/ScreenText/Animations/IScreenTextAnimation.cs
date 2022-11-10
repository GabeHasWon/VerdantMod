using Microsoft.Xna.Framework;

namespace Verdant.Systems.ScreenText.Animations
{
    internal interface IScreenTextAnimation
    {
        void ModifyDraw(float factor, ScreenText self, ref Vector2 position, ref Color drawColor, ref Color speakerColor, ref float scale);
    }
}
