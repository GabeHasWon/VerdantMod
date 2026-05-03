using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Verdant.Systems.ModCompat;

internal class MusicDisplayCalls : ModSystem
{
    private static readonly Asset<Texture2D> Backvine = ModContent.Request<Texture2D>("Verdant/Textures/MusicDisplayBackvine");

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
        AddMusic("Sounds/Music/TearRainEvent", "TearRainEvent");

        object x = display.Call("AddPreDraw", (Delegate)SpecialDraw, new short[] { MusicSlot("TearRain"), MusicSlot("ApotheosisLullaby"), MusicSlot("PetalsFall"), 
            MusicSlot("VibrantHorizon"), MusicSlot("TearRainEvent") });
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
            Vector2 pos = new(x + i * 30 - size.X / 2, y - 24);
            int frameId = (i % 3 + i + i / 2 + i % 4) % 4;
            Rectangle frame = new(frameId * 42, 0, 40, 44);
            float rotation = MathF.Sin(Main.GameUpdateCount * 0.08f + i * MathHelper.PiOver4 * 1.5f) * 0.5f * (i % 3 switch
            {
                0 => 1f,
                1 => 0.9f,
                _ => 0.75f,
            });

            Main.spriteBatch.Draw(Backvine.Value, pos, frame, Color.White * baseAlpha, rotation, frame.Size() / 2f, 1f, SpriteEffects.None, 0);
        }

        return true;
    }
}
