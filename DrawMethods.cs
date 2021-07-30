using Microsoft.Xna.Framework;
using On.Terraria;
using Terraria.ModLoader;
using Verdant.Backgrounds.BGItem;

namespace Verdant
{
    public partial class VerdantMod : Mod
    {
        private void Main_DrawBackgroundBlackFill(Main.orig_DrawBackgroundBlackFill orig, Terraria.Main self)
        {
            orig(self);

            if (Terraria.Main.playerLoaded && BackgroundItemManager.Loaded && !Terraria.Main.gameMenu)
                BackgroundItemManager.Draw();
        }

        private void Main_DrawPlayer(Main.orig_DrawPlayer orig, Terraria.Main self, Terraria.Player drawPlayer, Vector2 Position, float rotation, Vector2 rotationOrigin, float shadow)
        {
            orig(self, drawPlayer, Position, rotation, rotationOrigin, shadow);
            if (shadow == 0 && Terraria.Main.playerLoaded && !Terraria.Main.gameMenu)
                Foreground.ForegroundManager.Run();
        }
    }
}