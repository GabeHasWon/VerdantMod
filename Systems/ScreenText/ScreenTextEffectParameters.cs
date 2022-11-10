namespace Verdant.Systems.ScreenText
{
    internal readonly struct ScreenTextEffectParameters
    {
        public readonly float Timer;
        public readonly float Scale;
        public readonly float Scale2;

        public ScreenTextEffectParameters(float timer, float scale, float scale2)
        {
            Timer = timer;
            Scale = scale;
            Scale2 = scale2;
        }
    }
}
