using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Projectiles.Verdant.Misc
{
    class VerdantWisp : ModProjectile
    {
        private int _flameFrame = 0; //flame frame hehe

        ref float Rotation => ref projectile.ai[0];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wisp");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.width = 30;
            projectile.height = 38;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.magic = true;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center - new Vector2(0, 8), new Vector3(0.4f, 0.85f, 0.92f) * 1);

            projectile.velocity *= 0.98f;
            projectile.rotation = projectile.velocity.X * 0.04f;

            if (Main.rand.NextBool(9))
            {
                Dust d = Main.dust[Dust.NewDust(projectile.Center - new Vector2(2, 10), 4, 4, 59, 0, -6, 0, default, 1)]; //59 = BlueTorch
                d.fadeIn = 1f;
            }

            if (projectile.frameCounter++ == 3)
            {
                projectile.frameCounter = 0;
                _flameFrame++;
                if (_flameFrame > 2)
                    _flameFrame = 0;
            }

            Player p = Main.player[projectile.owner];
            projectile.Center = p.Center + new Vector2(0, p.gfxOffY) + new Vector2(0, 80 + (80 * (float)(Math.Sin(projectile.timeLeft * 0.002f) * 0.8f))).RotatedBy(Rotation);
            Rotation += 0.02f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //General function
            Texture2D proj = Main.projectileTexture[projectile.type];
            for (int k = projectile.oldPos.Length - 1; k >= 0; k--)
            {
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / projectile.oldPos.Length);
                spriteBatch.Draw(proj, projectile.oldPos[k] - Main.screenPosition + new Vector2(15, 19), new Rectangle(0, 0, 30, 38), color, projectile.rotation, new Vector2(15, 19), 1f, SpriteEffects.None, 1f);

                color = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - k) / projectile.oldPos.Length);
                spriteBatch.Draw(proj, projectile.oldPos[k] - Main.screenPosition + new Vector2(15, -6), new Rectangle(32 + (16 * _flameFrame), 0, 14, 36), color, projectile.rotation * 0.8f, new Vector2(7, 18), 1f, SpriteEffects.None, 1f);
                spriteBatch.Draw(proj, projectile.oldPos[k] - Main.screenPosition + new Vector2(15, -6), new Rectangle(32 + (16 * _flameFrame), 0, 14, 36), color * 0.9f, projectile.rotation * 0.8f, new Vector2(7, 18), 1.05f, SpriteEffects.None, 1f);
                spriteBatch.Draw(proj, projectile.oldPos[k] - Main.screenPosition + new Vector2(9, 9), new Rectangle(42, 40, 26, 4), color, 0f, new Vector2(7, 18), 1f, SpriteEffects.None, 1f);
            }
            return false;
        }
    }
}