using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Verdant.Systems.ModCompat;

internal class MusicDisplayCalls : ModSystem
{
    private static Asset<Texture2D> Backvine = ModContent.Request<Texture2D>("Verdant/Textures/MusicDisplayBackvine");

	public override void PostAddRecipes()
    {
        if (!ModLoader.TryGetMod("MusicDisplay", out Mod display))
            return;

        LocalizedText modName = Language.GetText("Mods.Verdant.MusicDisplay.ModName");

        void AddMusic(string path, string name)
        {
            LocalizedText author = Language.GetText("Mods.Verdant.MusicDisplay." + name + ".Author");
            LocalizedText displayName = Language.GetText("Mods.Verdant.MusicDisplay." + name + ".DisplayName");
            display.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, path), displayName, author, modName);
        }

        AddMusic("Sounds/Music/TearRain", "TearRain");
        AddMusic("Sounds/Music/ApotheosisLullaby", "ApotheosisLullaby");
        AddMusic("Sounds/Music/PetalsFall", "PetalsFall");
        AddMusic("Sounds/Music/VibrantHorizon", "VibrantHorizon");

        object x = display.Call("AddPreDraw", (Delegate)SpecialDraw, new short[] { MusicSlot("TearRain"), MusicSlot("ApotheosisLullaby"), MusicSlot("PetalsFall"), MusicSlot("VibrantHorizon") });
        return;

        short MusicSlot(string name)
        {
            return (short)MusicLoader.GetMusicSlot(Mod, "Sounds/Music/" + name);
        }
    }

    public static bool SpecialDraw(ref string nowText, ref string title, ref string author, ref string sub, ref float baseScale, Color[] colors, ref float delta, float defaultMaxDelta,
        ref float x, ref float y, ref Vector2 originMod, ref float baseAlpha, float? alwaysOn)
    {
        Vector2 size = FontAssets.DeathText.Value.MeasureString(title);
        size.X -= 90;

        for (int i = 0; i < size.X / 30; ++i)
        {
            Vector2 pos = new(x + i * 30 - size.X / 2, y - 54);
            int frameId = (i % 3 + i + i / 2 + i % 4) % 4;
            Rectangle frame = new(frameId * 42, 0, 40, 44);
            float rotation = MathF.Sin(Main.GameUpdateCount * 0.08f + i * MathHelper.PiOver4 * 1.5f) * 0.35f * (i % 3 switch
            {
                0 => 1.2f,
                1 => 1f,
                _ => 0.9f,
            });
            Main.spriteBatch.Draw(Backvine.Value, pos, frame, Color.White * baseAlpha, rotation, frame.Size() / 2f, 1f, SpriteEffects.None, 0);
        }

        return true;
    }

    public static bool SpecialDraw_Old(ref string nowText, ref string title, ref string author, ref string sub, ref float baseScale, Color[] colors, ref float delta, float defaultMaxDelta,
    ref float x, ref float y, ref Vector2 originMod, ref float baseAlpha, float? alwaysOn)
    {
        string newTitle = title;
        title = "";

        float factor = 1;

        if (delta < 4f)
            factor = delta / 4f;
        else if (delta > 6f)
            factor = 1 - (delta - 6f) / 4f;

        if (alwaysOn.HasValue)
            factor = 1;

        BuildString(ref newTitle);
        Vector2 size = FontAssets.DeathText.Value.MeasureString(newTitle);
        DrawString(newTitle, new Vector2(x, y - 42), colors[0] * baseAlpha, 0, size * originMod, new Vector2(0.85f) * baseScale);

        BuildString(ref author);
        BuildString(ref sub);
        BuildString(ref nowText);

        void BuildString(ref string s)
        {
            int len = (int)MathHelper.Clamp(MathHelper.Lerp(0, s.Length, factor), 0, s.Length);
            s = s[..len];
        }

        return true;
    }

    private static void DrawString(string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
     => ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.DeathText.Value, text, position, baseColor, rotation, origin, baseScale, maxWidth, spread);
}
