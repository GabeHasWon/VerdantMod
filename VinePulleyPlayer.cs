using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Verdant.Projectiles.Misc;

namespace Verdant
{
    public class VinePulleyPlayer : ModPlayer
    {
        public int currentVine = -1;
        public int vineTimer = 0;
        public float vineOffset = 0;

        public Projectile CurrentVine => Main.projectile[currentVine];
        public VineWandVine CurrentVineMod => Main.projectile[currentVine].modProjectile as VineWandVine;

        public override void PreUpdateMovement()
        {
            vineTimer--;

            if (currentVine != -1)
            {
                player.position = CurrentVineMod.PulleyPosition(player);
                player.velocity = Vector2.Zero;
                player.pulley = true;
                player.ChangeDir(1);

                if (player.controlJump)
                {
                    player.pulley = false;
                    player.pulleyFrame = (int)(Main.GameUpdateCount / 4) % 2;
                    player.velocity.Y -= 6;

                    currentVine = -1;
                    vineTimer = 0;
                    return;
                }
                else if (player.controlUp && CurrentVineMod.nextVine != -1)
                {
                    vineOffset -= 0.33f;

                    if (vineOffset < 0 && vineTimer < 0)
                    {
                        currentVine = CurrentVineMod.nextVine;
                        vineTimer = 2;
                        vineOffset = 1;
                    }
                }
                else if (player.controlDown && CurrentVineMod.priorVine != -1)
                {
                    vineOffset += 0.33f;

                    if (vineOffset > 1 && vineTimer < 0)
                    {
                        currentVine = CurrentVineMod.priorVine;
                        vineTimer = 2;
                        vineOffset = 0;
                    }
                }
            }
        }

        public static void Player_QuickMount(On.Terraria.Player.orig_QuickMount orig, Player self)
        {
            if (self.GetModPlayer<VinePulleyPlayer>().currentVine != -1)
            {
                self.GetModPlayer<VinePulleyPlayer>().currentVine = -1;
                self.GetModPlayer<VinePulleyPlayer>().vineTimer = 0;
            }

            orig(self);
        }
    }
}
