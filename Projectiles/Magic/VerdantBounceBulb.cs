using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Verdant.Projectiles.Magic
{
    class VerdantBounceBulb : ModProjectile
    {
        public const int MaxTimeLeft = 200;

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.width = 14;
            projectile.height = 14;
            projectile.penetrate = 1;
            projectile.timeLeft = MaxTimeLeft;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.damage = 1;
            projectile.aiStyle = 0;
        }

        public override bool CanHitPlayer(Player target) => true;

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            //target.velocity.Y = -16f;
            projectile.Kill();
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            damage = 0;
            crit = false;
            projectile.Kill();
        }

        public override void AI()
        {
            //projectile.velocity.Y -= 0.2f;
            if (projectile.ai[1] == 0)
            {
                projectile.ai[1] = Main.rand.Next(2) + 1;
                if (projectile.ai[1] == 1) projectile.velocity = projectile.velocity.RotatedBy(MathHelper.PiOver4);
                if (projectile.ai[1] == 2) projectile.velocity = projectile.velocity.RotatedBy(-MathHelper.PiOver4);
            }
            if (projectile.ai[1] == 1)
                projectile.velocity = projectile.velocity.RotatedBy(Math.Sin(-0.8f) * 0.02f);
            else
                projectile.velocity = projectile.velocity.RotatedBy(Math.Sin(0.8f) * 0.02f);

            if (projectile.timeLeft < MaxTimeLeft - 10)
                projectile.hostile = true;
        }
    }
}
