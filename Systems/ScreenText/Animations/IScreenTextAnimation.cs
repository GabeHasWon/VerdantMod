using Microsoft.Xna.Framework;

namespace Verdant.Systems.ScreenText.Animations;

public interface IScreenTextAnimation
{
    void ModifyDraw(float factor, ScreenText self, ref Vector2 position, ref Color drawColor, ref Color speakerColor, ref float scale);
}
