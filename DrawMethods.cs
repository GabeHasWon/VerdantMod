using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Verdant.Backgrounds.BGItem;

namespace Verdant
{
    public partial class VerdantMod : Mod
    {
        private void Main_DrawBackgroundBlackFill(On.Terraria.Main.orig_DrawBackgroundBlackFill orig, Main self)
        {
            orig(self);

            if (Main.PlayerLoaded && BackgroundItemManager.Loaded && !Main.gameMenu)
                BackgroundItemManager.Draw();
        }
    }
}