using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace Verdant.Projectiles.Misc
{
    class YellowPetalFloaterProj : ModProjectile
    {
        public bool BouncedUpon { get => Projectile.ai[0] != 0; set => Projectile.ai[0] = !value ? 0 : 1; }

        public ref float Timer => ref Projectile.ai[1];

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
            Projectile.rotation = (float)Math.Sin(Timer * 0.02f) * 0.2f;

            if (Projectile.timeLeft >= 15)
                return;

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
                            p.velocity.Y = -14;
                            BouncedUpon = true;
                            Timer = 0;
                            Projectile.scale = 0.5f;
                            break;
                        }
                    }
                }
            }
            else
            {
                Projectile.rotation *= 0.9f;

                if (Timer > 60)
                {
                    BouncedUpon = false;
                    Projectile.scale = 1f;
                    Timer = 0;
                }
            }
        }
    }
}
