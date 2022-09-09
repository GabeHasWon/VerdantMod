using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.Audio;

namespace Verdant.Projectiles.Misc
{
    class YellowPetalFloaterProj : ModProjectile
    {
        public bool BouncedUpon { get => Projectile.ai[0] != 0; set => Projectile.ai[0] = !value ? 0 : 1; }
        public ref float Timer => ref Projectile.ai[1];

        internal Vector2 anchor = Vector2.Zero;

        public override void SetStaticDefaults() => DisplayName.SetDefault("Yellow Petal Flower");

        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.width = 58;
            Projectile.height = 38;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 25;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.9f;
            Timer++;
            Projectile.rotation = (float)Math.Sin(Timer * 0.04f) * 0.3f;

            if (Projectile.timeLeft >= 15)
                return;

            Projectile.timeLeft = 3;

            if (Main.rand.NextBool(14))
                Dust.NewDustPerfect(new Vector2(Projectile.position.X + 4 + Main.rand.Next(Projectile.width - 8), Projectile.Center.Y), DustID.Cloud, new Vector2(0, Main.rand.NextFloat(0.2f, 0.5f)));

            Rectangle top = new((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, 8);

            if (!BouncedUpon)
            {
                for (int i = 0; i < Main.maxPlayers; ++i)
                {
                    Player p = Main.player[i];
                    if (p.active && !p.dead)
                    {
                        Rectangle pBottom = new((int)p.position.X, (int)p.Bottom.Y, p.width, 4);
                        if (top.Intersects(pBottom) && p.velocity.Y > 0 && BounceOnPlayer(p))
                            break;
                    }
                }
            }
            else
            {
                Projectile.velocity += Projectile.DirectionTo(anchor) * 0.5f;
                Projectile.scale = (Timer / 60f) * 0.25f + 0.75f;

                if (Projectile.DistanceSQ(anchor) < 2 * 2)
                    Projectile.velocity *= 0.98f;

                if (Timer > 60)
                    BouncedUpon = false;
            }
        }

        private bool BounceOnPlayer(Player p)
        {
            if (!p.controlDown) //Jump up
            {
                Projectile.velocity = new Vector2(p.velocity.X * 0.2f, 4 + p.velocity.Y * 0.5f);
                p.velocity.Y = -14;
                p.velocity.X *= 1.2f;
                p.fallStart = (int)(Projectile.position.Y / 16f);

                BouncedUpon = true;
                Timer = 0;

                for (int j = 0; j < 14; ++j)
                    Dust.NewDust(Projectile.position + new Vector2(4, 12), Projectile.width - 8, 8, DustID.Cloud, Main.rand.NextFloat(-0.2f, 0.2f) + Projectile.velocity.X, Main.rand.NextFloat(2, 3f));

                SoundEngine.PlaySound(new SoundStyle("Verdant/Sounds/CloudJump") with { PitchVariance = 0.05f }, Projectile.Center);
                return true;
            }
            else //Fall through
            {
                Projectile.velocity = new Vector2(p.velocity.X * 0.2f, 4 + p.velocity.Y * 0.5f) * 0.25f;
                p.velocity.Y *= 0.5f;
                p.fallStart = (int)(Projectile.position.Y / 16f);

                for (int j = 0; j < 14; ++j)
                    Dust.NewDust(Projectile.position + new Vector2(4, 12), Projectile.width - 8, 8, DustID.Cloud, Main.rand.NextFloat(-0.2f, 0.2f) + Projectile.velocity.X, Main.rand.NextFloat(2, 3f));

                SoundEngine.PlaySound(new SoundStyle("Verdant/Sounds/CloudLand") with { PitchVariance = 0.05f }, Projectile.Center);

                BouncedUpon = true;
                Timer = 40;
            }
            return false;
        }
    }
}
