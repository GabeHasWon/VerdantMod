using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;

namespace Verdant.Projectiles.Misc
{
    public class VineWandVine : ModProjectile
    {
        public int nextVine = -1;
        public int priorVine = -1;

        public ref float Timer => ref Projectile.ai[0];
        public ref float VineIndex => ref Projectile.ai[1];

        public Projectile NextVine => Main.projectile[nextVine];
        public Projectile PriorVine => Main.projectile[priorVine];

        public bool perm = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vine");

            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.width = 20;
            Projectile.height = 18;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 10 * 60;
        }

        public override void AI()
        {
            Player p = Main.player[Projectile.owner];
            p.heldProj = Projectile.whoAmI;

            if (p.whoAmI != Main.myPlayer)
                return; //mp check (hopefully)

            if (priorVine != -1 && InvalidVine(true))
                priorVine = -1;

            if (nextVine != -1 && InvalidVine(false))
                nextVine = -1;

            int dir = (Projectile.whoAmI % 2) + (Projectile.whoAmI % 9) + (Projectile.whoAmI % 3); //"randomize" direction
            Projectile.spriteDirection = (dir % 2 == 0) ? -1 : 1;

            float rotOff = (float)Math.Sin((Timer++ + (VineIndex * 12)) * 0.05f) * 0.2f;
            if (priorVine != -1)
                Projectile.rotation = Projectile.AngleTo(PriorVine.Center) - MathHelper.PiOver2 + rotOff;
            else if (nextVine != -1)
                Projectile.rotation = Projectile.AngleTo(NextVine.Center) - MathHelper.PiOver2 + rotOff;

            Rectangle playerTop = new((int)p.position.X, (int)p.position.Y, p.width, 2);

            if (playerTop.Intersects(Projectile.Hitbox) && (p.controlUp || p.controlDown) && !p.controlJump && !p.pulley && p.grappling[0] < 0 && !p.mount.Active && !Collision.SolidCollision(p.position, p.width, p.height) && Projectile.timeLeft > 3)
            {
                p.pulley = true;
                p.pulleyDir = 1;
                p.position = Projectile.position;
                p.fallStart = (int)(Projectile.position.Y / 16f);

                p.GetModPlayer<VinePulleyPlayer>().currentVine = Projectile.whoAmI;
            }

            if (perm)
                Projectile.timeLeft = 10;

            bool invalidNext = nextVine == -1 || InvalidVine(false);
            bool invalidPrior = priorVine == -1 || InvalidVine(true);
            if (invalidNext && invalidPrior)
                Projectile.Kill();
        }

        private bool InvalidVine(bool prior)
        {
            Projectile vine = prior ? PriorVine : NextVine;
            return !vine.active || vine.ModProjectile is not VineWandVine modVine || (prior ? modVine.nextVine : modVine.priorVine) != Projectile.whoAmI;
        }

        public void PulleyVelocity(Player player)
        {
            if (player.controlDown && player.controlUp)
                return;

            const float Speed = 4.75f;

            if (nextVine != -1 && player.controlUp)
                player.velocity = player.DirectionTo(NextVine.Center) * Speed;
            if (priorVine != -1 && player.controlDown)
                player.velocity = player.DirectionTo(PriorVine.Center) * Speed;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; ++i)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, 0, 0);
        }
    }
}
