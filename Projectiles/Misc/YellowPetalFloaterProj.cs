using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;

namespace Verdant.Projectiles.Misc
{
    class YellowPetalFloaterProj : ModProjectile
    {
        public bool BouncedUpon { get => Projectile.ai[0] != 0; set => Projectile.ai[0] = !value ? 0 : 1; }
        public ref float Timer => ref Projectile.ai[1];

        internal Vector2 anchor = Vector2.Zero;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yellow Petal Flower");
        }

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

            if (Main.rand.NextBool(14))
                Dust.NewDustPerfect(new Vector2(Projectile.position.X + 4 + Main.rand.Next(Projectile.width - 8), Projectile.Center.Y), DustID.Cloud, new Vector2(0, Main.rand.NextFloat(0.2f, 0.5f)));

            Rectangle top = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, 8);
            Projectile.timeLeft++;

            if (!BouncedUpon)
            {
                for (int i = 0; i < Main.maxPlayers; ++i)
                {
                    Player p = Main.player[i];
                    if (p.active && !p.dead)
                    {
                        Rectangle pBottom = new Rectangle((int)p.position.X, (int)p.Bottom.Y, p.width, 4);
                        if (top.Intersects(pBottom) && p.velocity.Y > 0)
                        {
                            Projectile.scale = 0.5f;
                            Projectile.velocity = new Vector2(p.velocity.X * 0.2f, 4 + p.velocity.Y * 0.5f);

                            p.velocity.Y = -14;
                            p.velocity.X *= 1.2f;
                            p.fallStart = (int)(Projectile.position.Y / 16f);

                            BouncedUpon = true;
                            Timer = 0;

                            for (int j = 0; j < 8; ++j)
                                Dust.NewDust(Projectile.position + new Vector2(4, 0), Projectile.width - 8, 8, DustID.Cloud, 0, 2);
                            break;
                        }
                    }
                }
            }
            else
            {
                Projectile.velocity += Projectile.DirectionTo(anchor) * 0.5f;

                if (Projectile.DistanceSQ(anchor) < 2 * 2)
                    Projectile.velocity *= 0.98f;

                if (Timer > 60)
                {
                    BouncedUpon = false;
                    Projectile.scale = 1f;
                }
            }
        }
    }
}
