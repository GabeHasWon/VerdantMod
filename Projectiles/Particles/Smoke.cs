using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Verdant.Drawing;

namespace Verdant.Projectiles.Particles
{
    class Smoke : ModProjectile
    {
        private Color drawCol = Color.Green;
        private int _maxTimeLeft = 0;

        // public override void SetStaticDefaults() => DisplayName.SetDefault("Smoke");

        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.width = 30;
            Projectile.height = 38;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = Main.rand.NextFloat(0.9f, 1.1f);
            Projectile.timeLeft = (int)(Main.rand.Next(30, 60) * Projectile.scale);
            Projectile.frame = Main.rand.Next(4);

            _maxTimeLeft = Projectile.timeLeft;
        }

        public override void AI()
        {
            Projectile.rotation += 0.05f * Projectile.velocity.Length();
            Projectile.velocity *= 0.92f;

            float section = _maxTimeLeft / 5f;
            float time = (_maxTimeLeft - Projectile.timeLeft) % section / section;
            int currentSection = (int)((_maxTimeLeft - Projectile.timeLeft) / section);

            if (currentSection == 0)
                drawCol = Color.Lerp(Color.Yellow, Color.Orange, time);
            else if (currentSection == 1)
                drawCol = Color.Lerp(Color.Orange, Color.Red, time);
            else if (currentSection == 2)
                drawCol = Color.Lerp(Color.Red, new Color(100, 100, 100), time);
            else if (currentSection == 3)
                drawCol = Color.Lerp(new Color(100, 100, 100), new Color(30, 30, 30), time);
            else if (currentSection == 4)
                drawCol = Color.Lerp(new Color(30, 30, 30), Color.Black * 0, time);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var frame = new Rectangle(0, Projectile.frame * 42, 38, 40);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, frame, drawCol * (Projectile.scale - 0.8f), Projectile.rotation, new(19, 20), Projectile.scale, SpriteEffects.None, 1f);
            return false;
        }
    }
}
