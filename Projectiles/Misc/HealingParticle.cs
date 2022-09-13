using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Verdant.Drawing;

namespace Verdant.Projectiles.Misc
{
    class HealingParticle : ModProjectile, IDrawAdditive
    {
        public ref float Timer => ref Projectile.ai[0];

        private Color drawCol = Color.Green;

        public override void SetStaticDefaults() => DisplayName.SetDefault("Healing Particle");

        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.width = 30;
            Projectile.height = 38;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2000;
            Projectile.scale = Main.rand.NextFloat(0.1f, 0.3f);

            drawCol = new Color(Main.rand.NextFloat(0.2f), Main.rand.NextFloat(0.25f, 0.9f), Main.rand.NextFloat(0.2f));
        }

        public override void AI()
        {
            Timer++;

            if (Timer < 120)
                Projectile.velocity *= 0.97f;
            else
            {
                Projectile.velocity = Projectile.DirectionTo(Projectile.Owner().Center) * MathHelper.Clamp((Timer - 120) / 2.5f, 0f, 30f);

                if (Projectile.DistanceSQ(Projectile.Owner().Center) < 20 * 20)
                    Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor) => false;

        public void DrawAdditive(AdditiveLayer layer)
        {
            if (layer != AdditiveLayer.AfterPlayer)
                return;

            Texture2D tex = Mod.Assets.Request<Texture2D>("Textures/Circle").Value;
            float rot = Projectile.velocity.ToRotation();
            Vector2 scale = new Vector2(1 + (Projectile.velocity.Length() * 0.075f), 1) * Projectile.scale;

            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, drawCol * 0.8f, rot, tex.Size() / 2f, scale, SpriteEffects.None, 1f);
        }
    }
}
