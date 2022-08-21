using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Verdant.Projectiles.Misc;

namespace Verdant
{
    public class VinePulleyPlayer : ModPlayer
    {
        public const int MaxVineResource = 80;

        public int currentVine = -1;
        public int vineTimer = 0;
        public float vineOffset = 0;

        public int vineResource = 0;
        public int vineRegenCooldown = 0;

        public Projectile CurrentVine => Main.projectile[currentVine];
        public VineWandVine CurrentVineMod => Main.projectile[currentVine].ModProjectile as VineWandVine;

        public override void ResetEffects()
        {
            if (vineRegenCooldown-- < 0 && vineRegenCooldown % 5 == 0 && vineResource < MaxVineResource)
                vineResource++;
        }

        public int VineCount() => Main.projectile.Where(x => x.active && x.owner == Player.whoAmI && x.type == ModContent.ProjectileType<VineWandVine>()).Count();

        public override void PreUpdateMovement()
        {
            const float ClimbSpeed = 0.5f;

            vineTimer--;

            if (currentVine != -1)
            {
                if (!CurrentVine.active || CurrentVine.timeLeft < 5)
                {
                    currentVine = -1;
                    return;
                }

                Player.position = CurrentVineMod.PulleyPosition(Player);
                Player.velocity = Vector2.Zero;
                Player.pulley = true;
                Player.ChangeDir(1);

                if (Player.controlJump)
                {
                    Player.pulley = false;
                    Player.pulleyFrame = (int)(Main.GameUpdateCount / 4) % 2;
                    Player.velocity.Y -= 6;

                    currentVine = -1;
                    vineTimer = 0;
                    return;
                }
                else if (Player.controlUp && CurrentVineMod.nextVine != -1)
                {
                    vineOffset -= ClimbSpeed;

                    if (Collision.SolidCollision(CurrentVineMod.PulleyPosition(Player), Player.width, Player.height))
                        vineOffset += ClimbSpeed;

                    if (vineOffset < 0 && vineTimer < 0)
                    {
                        currentVine = CurrentVineMod.nextVine;
                        vineTimer = 2;
                        vineOffset = 1;
                    }
                }
                else if (Player.controlDown && CurrentVineMod.priorVine != -1)
                {
                    vineOffset += ClimbSpeed;

                    if (Collision.SolidCollision(CurrentVineMod.PulleyPosition(Player), Player.width, Player.height))
                        vineOffset -= ClimbSpeed;

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
