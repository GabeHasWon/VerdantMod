using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using On.Terraria;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Backgrounds.BGItem;

namespace Verdant
{
    public partial class VerdantMod : Mod
    {
        public BaseBGItem BGItemHandler;

        private void Main_DrawBackgroundBlackFill(Main.orig_DrawBackgroundBlackFill orig, Terraria.Main self)
        {
            orig(self);

            bool playerInv = Terraria.Main.hasFocus && (!Terraria.Main.autoPause || Terraria.Main.netMode != NetmodeID.SinglePlayer ||
                (Terraria.Main.autoPause && !Terraria.Main.playerInventory && Terraria.Main.netMode == NetmodeID.SinglePlayer));
            if (Terraria.Main.playerLoaded && BGItemHandler != null)
                BGItemHandler.RunAll(playerInv);
        }

        private void Main_DrawPlayer(Main.orig_DrawPlayer orig, Terraria.Main self, Terraria.Player drawPlayer, Vector2 Position, float rotation, Vector2 rotationOrigin, float shadow)
        {
            orig(self, drawPlayer, Position, rotation, rotationOrigin, shadow);
            if (Terraria.Main.playerLoaded)
            {
                //new Vector2(Main.screenWidth - 4, Main.screenHeight - 4) / 2
                Foreground.ForegroundManager.Run();
            }
        }
    }
}