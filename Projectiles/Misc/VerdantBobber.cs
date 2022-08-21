using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Projectiles.Misc
{
    public class VerdantBobber : ModProjectile
    {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Verdant Bobber");

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BobberWooden);
            DrawOriginOffsetY = 0;
        }

        public override bool PreDrawExtras()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0.5f, 0.16f, 0.30f) * 3f);

            int xPositionAdditive = 38;
            float yPositionAdditive = 33f;

            Player player = Main.player[Projectile.owner];
            if (!Projectile.bobber || player.inventory[player.selectedItem].holdStyle <= 0)
                return false;

            Vector2 lineOrigin = player.MountedCenter;
            lineOrigin.Y += player.gfxOffY;
            int type = player.inventory[player.selectedItem].type;
            //This variable is used to account for Gravitation Potions
            float gravity = player.gravDir;

            if (type == ModContent.ItemType<Items.Verdant.Tools.VerdantFishingRod>())
            {
                lineOrigin.X += xPositionAdditive * player.direction;
                if (player.direction < 0)
                    lineOrigin.X -= 13f;
                lineOrigin.Y -= yPositionAdditive * gravity;
            }

            if (gravity == -1f)
                lineOrigin.Y -= 12f;

            lineOrigin = player.RotatedRelativePoint(lineOrigin + new Vector2(8f), true) - new Vector2(8f);
            Vector2 playerToProjectile = Projectile.Center - lineOrigin;
            bool canDraw = true;

            if (playerToProjectile.X == 0f && playerToProjectile.Y == 0f)
                return false;

            float playerToProjectileMagnitude = playerToProjectile.Length();
            playerToProjectileMagnitude = 12f / playerToProjectileMagnitude;
            playerToProjectile *= playerToProjectileMagnitude;
            lineOrigin -= playerToProjectile;
            playerToProjectile = Projectile.Center - lineOrigin;

            while (canDraw)
            {
                float height = 12f;
                float positionMagnitude = playerToProjectile.Length();

                if (float.IsNaN(positionMagnitude))
                    break;

                if (positionMagnitude < 20f)
                {
                    height = positionMagnitude - 8f;
                    canDraw = false;
                }

                playerToProjectile *= 12f / positionMagnitude;
                lineOrigin += playerToProjectile;
                playerToProjectile.X = Projectile.position.X + Projectile.width * 0.5f - lineOrigin.X;
                playerToProjectile.Y = Projectile.position.Y + Projectile.height * 0.1f - lineOrigin.Y;

                if (positionMagnitude > 12f)
                {
                    float positionInverseMultiplier = 0.3f;
                    float absVelocitySum = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);

                    if (absVelocitySum > 16f)
                        absVelocitySum = 16f;

                    absVelocitySum = 1f - absVelocitySum / 16f;
                    positionInverseMultiplier *= absVelocitySum;
                    absVelocitySum = positionMagnitude / 80f;

                    if (absVelocitySum > 1f)
                        absVelocitySum = 1f;

                    positionInverseMultiplier *= absVelocitySum;

                    if (positionInverseMultiplier < 0f)
                        positionInverseMultiplier = 0f;

                    absVelocitySum = 1f - Projectile.localAI[0] / 100f;
                    positionInverseMultiplier *= absVelocitySum;

                    if (playerToProjectile.Y > 0f)
                    {
                        playerToProjectile.Y *= 1f + positionInverseMultiplier;
                        playerToProjectile.X *= 1f - positionInverseMultiplier;
                    }
                    else
                    {
                        absVelocitySum = Math.Abs(Projectile.velocity.X) / 3f;
                        if (absVelocitySum > 1f)
                            absVelocitySum = 1f;
                        absVelocitySum -= 0.5f;
                        positionInverseMultiplier *= absVelocitySum;
                        if (positionInverseMultiplier > 0f)
                            positionInverseMultiplier *= 2f;
                        playerToProjectile.Y *= 1f + positionInverseMultiplier;
                        playerToProjectile.X *= 1f - positionInverseMultiplier;
                    }
                }

                Color lineColor = Lighting.GetColor((int)lineOrigin.X / 16, (int)(lineOrigin.Y / 16f), Color.White);
                float rotation = playerToProjectile.ToRotation() - MathHelper.PiOver2;
                Main.spriteBatch.Draw(TextureAssets.FishingLine.Value, new Vector2(lineOrigin.X - Main.screenPosition.X + TextureAssets.FishingLine.Value.Width * 0.5f, lineOrigin.Y - Main.screenPosition.Y + TextureAssets.FishingLine.Value.Height * 0.5f), new Rectangle(0, 0, TextureAssets.FishingLine.Value.Width, (int)height), lineColor, rotation, new Vector2(TextureAssets.FishingLine.Value.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}