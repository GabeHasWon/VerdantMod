using Terraria.Localization;

namespace Verdant
{
    public class VerdantLocalization
    {
        /// <summary>Allows detours to add other translations easily.</summary>
        public static string ScreenTextLocalization(string text)
        {
            if (text.StartsWith('$'))
                return Language.GetTextValue(text[1..]);
            return text;
        }
    }
}