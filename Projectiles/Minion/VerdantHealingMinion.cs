using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Drawing;

namespace Verdant.Projectiles.Minion
{
    class VerdantHealingMinion : ModProjectile, IDrawAdditive
    {
        ref float State => ref projectile.ai[0];

        public Vector2 goPosition = -Vector2.One;

        private int timer = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lush Flower");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.width = 48;
            projectile.height = 48;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.minionSlots = 1f;
            projectile.minion = true;
        }

        public override bool? CanCutTiles() => false;
        public override bool MinionContactDamage() => false;

        public override void AI()
        {
            timer++;

            float sc = 0.5f - (float)(Math.Sin(timer * 0.03f) * 0.1f);

            if (projectile.velocity.Length() < 3f)
                Lighting.AddLight(projectile.Center, new Vector3(0.4f, 0.12f, 0.24f) * 6 * ((3f - projectile.velocity.Length()) / 3) * sc);

            for (int i = 0; i < Main.player.Length; ++i)
            {
                Player p = Main.player[i];
                Vector2 off = new Vector2(0, (float)(Math.Sin(timer * 0.03f) * 6));
                float rad = .9f - ((float)(Math.Sin(timer * 0.03f) * 0.05f));

                float radMult = 1f;
                if (State == 1) radMult = 1.2f;

                if (timer % 210 == 0 && p.active && !p.dead && Vector2.Distance(p.MountedCenter, projectile.Center - off) < rad * radMult * 196)
                {
                    if (p.statLife < p.statLifeMax2 - 10)
                    {
                        p.HealEffect(10, true);
                        p.statLife += 10;
                    }
                }
            }

            State = Main.player.Count(x => x.active && !x.dead && x.HasBuff(ModContent.BuffType<Buffs.Minion.HealingFlowerBuff>())) - 1;
            if (State < 0)
                projectile.Kill();

            if (projectile.frameCounter++ % 30 == 0)
                projectile.frame = projectile.frame == 0 ? 1 : 0;

            if (goPosition != -Vector2.One)
            {
                projectile.velocity = (goPosition - projectile.position) * 0.055f;
                if (Vector2.Distance(projectile.position, goPosition) < 10)
                    goPosition = -Vector2.One;
            }

            projectile.velocity *= 0.85f;
            projectile.timeLeft = 60;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Summon anim
            float scale = 1;

            //General function
            Texture2D proj = ModContent.GetTexture("Verdant/Projectiles/Minion/VerdantHealingMinion");
            if (State <= 1) //Normal minion
            {
                scale *= State == 0 ? 1f : 1.2f; //Larger when stacked

                DrawCircle(spriteBatch, lightColor, scale, out Vector2 off);

                if (goPosition != -Vector2.One) //Indicator for move position
                    spriteBatch.Draw(proj, goPosition - Main.screenPosition - off + new Vector2(24), proj.Frame(2, 1, projectile.frame), Color.White * 0.2f, 0f, new Vector2(24), scale, SpriteEffects.None, 1f);

                for (int k = projectile.oldPos.Length - 1; k >= 0; k--)
                {
                    Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / projectile.oldPos.Length);
                    spriteBatch.Draw(proj, projectile.oldPos[k] - Main.screenPosition - off + new Vector2(24), proj.Frame(2, 1, projectile.frame), color, 0f, new Vector2(24), scale, SpriteEffects.None, 1f);
                }
            }
            return false;
        }

        private void DrawCircle(SpriteBatch spriteBatch, Color lightColor, float scale, out Vector2 off)
        {
            Texture2D circle = ModContent.GetTexture("Verdant/Projectiles/Minion/VerdantHealingCircle"); Sandstorm.TimeLeft = 1800;
            float sc = 1.9f - (float)(Math.Sin(timer * 0.03f) * 0.05f);
            float alphaScale = (sc - 1.78f) * 8;
            Color circleCol = projectile.GetAlpha(lightColor) * alphaScale;

            off = new Vector2(0, (float)(Math.Sin(timer * 0.03f) * 6));

            spriteBatch.Draw(circle, projectile.Center - Main.screenPosition - off, circle.Frame(), circleCol, timer * 0.006f, circle.Bounds.Center.ToVector2(), sc * scale, SpriteEffects.None, 1f);
            if (alphaScale > 0.95f)
            {
                spriteBatch.Draw(circle, projectile.Center - Main.screenPosition - off, circle.Frame(), projectile.GetAlpha(lightColor) * ((alphaScale - 0.95f) * 1.5f), timer * 0.006f, circle.Bounds.Center.ToVector2(), sc * scale * 1.04f, SpriteEffects.None, 1f);
                spriteBatch.Draw(circle, projectile.Center - Main.screenPosition - off, circle.Frame(), projectile.GetAlpha(lightColor) * ((alphaScale - 0.95f) * 1.5f), timer * 0.006f, circle.Bounds.Center.ToVector2(), sc * scale * 0.96f, SpriteEffects.None, 1f);
            }
        }

        public void DrawAdditive(AdditiveLayer layer)
        {
            if (layer == AdditiveLayer.AfterPlayer)
            {
                Texture2D tex = mod.GetTexture("Textures/Circle");
                float rot = projectile.velocity.ToRotation();
                Vector2 scale = new Vector2(1 + (projectile.velocity.Length() * 0.02f), 1) * 5f;
                float sc = 0.9f - (float)(Math.Sin(timer * 0.03f) * 0.05f);

                Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, new Color(0, 170, 62) * 0.45f, rot, tex.Size() / 2f, scale * sc, SpriteEffects.None, 1f);
            }
        }
    }
}
