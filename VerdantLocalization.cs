using Terraria.Localization;

namespace Verdant;

public class VerdantLocalization
{
    /// <summary>Allows detours to add other translations easily.</summary>
    public static string ScreenTextLocalization(string text)
    {
        if (text.StartsWith('$'))
            return Language.GetTextValue(text[1..]);

        return text;
    }

    public static LocalizedText ScreenTextLocalized(string text)
    {
        if (text.StartsWith('$'))
            return Language.GetText(text[1..]);

        return LocalizedText.Empty;
    }
}