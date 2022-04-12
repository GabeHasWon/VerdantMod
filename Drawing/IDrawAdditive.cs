using Terraria;

namespace Verdant.Drawing
{
    public enum AdditiveLayer
    {
        BeforePlayer = 0,
        AfterPlayer = 1
    }

    interface IDrawAdditive
    {
        void DrawAdditive(AdditiveLayer layer);
    }
}
