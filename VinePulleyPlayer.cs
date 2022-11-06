using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Projectiles.Misc;

namespace Verdant
{
    public class VinePulleyPlayer : ModPlayer
    {
        public const int MaxVineResource = 80;

        public int vineTimer = 0;
        public float vineOffset = 0;

        public int vineResource = 0;
        public int vineRegenCooldown = 0;

        public EnchantedVine CurrentVine = null;

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

            if (CurrentVine is not null)
            {
                if (CurrentVine.lifeTimer < 5 || CurrentVine.killMe || Player.dead || Player.DistanceSQ(CurrentVine.Center) > 80 * 80)
                {
                    CurrentVine = null;
                    return;
                }

                Player.velocity = Vector2.Zero;
                CurrentVine.PulleyVelocity(Player);
                Player.fallStart = (int)(CurrentVine.position.Y / 16f);
                Player.pulley = true;
                Player.pulleyDir = 0;

                if (Player.velocity.X != 0)
                {
                    Player.ChangeDir(Math.Sign(Player.velocity.X));
                    Player.pulleyFrameCounter++;

                    if (Player.pulleyFrameCounter % 10 == 0)
                        Player.pulleyFrame = Player.pulleyFrame == 0 ? 1 : 0;
                }

                if (!Player.controlDown && !Player.controlUp)
                    Player.velocity *= 0.9f;

                if (Player.controlJump)
                {
                    Player.pulley = false;
                    Player.pulleyFrame = (int)(Main.GameUpdateCount / 4) % 2;
                    Player.velocity.Y -= 6;

                    CurrentVine = null;
                    vineTimer = 0;
                    return;
                }
                else if (Player.controlUp && !CurrentVine.InvalidVine(false))
                {
                    vineOffset -= Player.velocity.Length();

                    if (Collision.SolidCollision(Player.position, Player.width, Player.height))
                        vineOffset += ClimbSpeed;

                    if (vineOffset < 0 && vineTimer < 0)
                    {
                        CurrentVine = CurrentVine.NextVine;
                        vineTimer = 2;
                        vineOffset = 1;
                    }
                }
                else if (Player.controlDown && !CurrentVine.InvalidVine(true))
                {
                    vineOffset += Player.velocity.Length();

                    if (Collision.SolidCollision(Player.position, Player.width, Player.height))
                        vineOffset -= ClimbSpeed;

                    if (vineOffset > 1 && vineTimer < 0)
                    {
                        CurrentVine = CurrentVine.PriorVine;
                        vineTimer = 2;
                        vineOffset = 0;
                    }
                }
            }
        }

        public static void Player_QuickMount(On.Terraria.Player.orig_QuickMount orig, Player self)
        {
            if (self.GetModPlayer<VinePulleyPlayer>().CurrentVine != null)
            {
                self.GetModPlayer<VinePulleyPlayer>().CurrentVine = null;
                self.GetModPlayer<VinePulleyPlayer>().vineTimer = 0;
            }

            orig(self);
        }

        public static void Player_Teleport(On.Terraria.Player.orig_Teleport orig, Player self, Vector2 newPos, int Style, int extraInfo)
        {
            if (self.GetModPlayer<VinePulleyPlayer>().CurrentVine != null)
            {
                self.GetModPlayer<VinePulleyPlayer>().CurrentVine = null;
                self.GetModPlayer<VinePulleyPlayer>().vineTimer = 0;
            }

            orig(self, newPos, Style, extraInfo);
        }
    }
}
