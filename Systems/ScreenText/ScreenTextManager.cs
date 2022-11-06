using System;

namespace Verdant.Systems.ScreenText
{
    internal class ScreenTextManager
    {
        public static string Speaker = string.Empty;
        public static ScreenText CurrentText = null;

        public static void Update()
        {
            if (CurrentText != null)
            {
                CurrentText.Update();

                if (!CurrentText.active)
                    CurrentText = CurrentText.Next;
            }
        }

        public static void Draw()
        {
            CurrentText?.Draw();
        }

        internal static void DrawAdditive()
        {
            CurrentText?.Draw();
        }
    }
}
