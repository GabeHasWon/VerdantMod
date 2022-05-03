using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Verdant.Drawing;

namespace Verdant.Projectiles.Misc
{
    class HealingParticle : ModProjectile, IDrawAdditive
    {
        public ref float Timer => ref projectile.ai[0];

        private Color drawCol = Color.Green;

        public override void SetStaticDefaults() => DisplayName.SetDefault("Healing Particle");

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.width = 30;
            projectile.height = 38;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 2000;
            projectile.scale = Main.rand.NextFloat(0.1f, 0.6f);Projectile.NewProjectile(default, default, 1, default, default);

            drawCol = new Color(Main.rand.NextFloat(0.2f), Main.rand.NextFloat(0.25f, 0.9f), Main.rand.NextFloat(0.2f));
        }

        public override void AI()
        {
            Timer++;

            if (Timer < 120)
                projectile.velocity *= 0.97f;
            else
            {
                projectile.velocity = projectile.DirectionTo(projectile.Owner().Center) * MathHelper.Clamp((Timer - 120) / 2.5f, 0f, 30f);

                if (projectile.DistanceSQ(projectile.Owner().Center) < 20 * 20)
                    projectile.Kill();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;

        public void DrawAdditive(AdditiveLayer layer)
        {
            if (layer != AdditiveLayer.AfterPlayer)
                return;

            Texture2D tex = mod.GetTexture("Textures/Circle");
            float rot = projectile.velocity.ToRotation();
            Vector2 scale = new Vector2(1 + (projectile.velocity.Length() * 0.075f), 1) * projectile.scale;

            Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, drawCol * 0.8f, rot, tex.Size() / 2f, scale, SpriteEffects.None, 1f);
        }
    }
}
