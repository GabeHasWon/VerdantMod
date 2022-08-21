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
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.penetrate = 1;
            Projectile.timeLeft = MaxTimeLeft;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.damage = 1;
            Projectile.aiStyle = 0;
        }

        public override bool CanHitPlayer(Player target) => true;

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            //target.velocity.Y = -16f;
            Projectile.Kill();
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            damage = 0;
            crit = false;
            Projectile.Kill();
        }

        public override void AI()
        {
            //projectile.velocity.Y -= 0.2f;
            if (Projectile.ai[1] == 0)
            {
                Projectile.ai[1] = Main.rand.Next(2) + 1;
                if (Projectile.ai[1] == 1) Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver4);
                if (Projectile.ai[1] == 2) Projectile.velocity = Projectile.velocity.RotatedBy(-MathHelper.PiOver4);
            }
            if (Projectile.ai[1] == 1)
                Projectile.velocity = Projectile.velocity.RotatedBy(Math.Sin(-0.8f) * 0.02f);
            else
                Projectile.velocity = Projectile.velocity.RotatedBy(Math.Sin(0.8f) * 0.02f);

            if (Projectile.timeLeft < MaxTimeLeft - 10)
                Projectile.hostile = true;
        }
    }
}
