using Microsoft.Xna.Framework;

namespace Verdant.Systems.ScreenText.Animations
{
    internal interface IScreenTextAnimation
    {
        void ModifyDraw(ref Vector2 position, ref Color drawColor, ref float scale);
    }
}
