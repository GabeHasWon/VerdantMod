using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace Verdant.Projectiles.Misc
{
    class YellowPetalFloaterProj : ModProjectile
    {
        public bool BouncedUpon { get => projectile.ai[0] != 0; set => projectile.ai[0] = !value ? 0 : 1; }

        public ref float Timer => ref projectile.ai[1];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yellow Petal Flower");
        }

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.width = 58;
            projectile.height = 38;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 25;
        }

        public override void AI()
        {
            projectile.velocity *= 0.9f;
            Timer++;
            projectile.rotation = (float)Math.Sin(Timer * 0.02f) * 0.2f;

            if (projectile.timeLeft >= 15)
                return;

            Rectangle top = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, 8);
            projectile.timeLeft++;

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
                            projectile.scale = 0.5f;
                            break;
                        }
                    }
                }
            }
            else
            {
                projectile.rotation *= 0.9f;

                if (Timer > 60)
                {
                    BouncedUpon = false;
                    projectile.scale = 1f;
                    Timer = 0;
                }
            }
        }
    }
}
