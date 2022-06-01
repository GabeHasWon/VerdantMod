using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Verdant.Projectiles.Misc
{
    public class VineWandVine : ModProjectile
    {
        public int nextVine = -1;
        public int priorVine = -1;

        public ref float Timer => ref projectile.ai[0];
        public ref float VineIndex => ref projectile.ai[1];

        public Projectile NextVine => Main.projectile[nextVine];
        public Projectile PriorVine => Main.projectile[priorVine];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vine");

            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.width = 20;
            projectile.height = 18;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 10 * 60;
        }

        public override void AI()
        {
            Player p = Main.player[projectile.owner];
            p.heldProj = projectile.whoAmI;

            if (p.whoAmI != Main.myPlayer)
                return; //mp check (hopefully)

            if (priorVine != -1 && !PriorVine.active)
                priorVine = 1;

            if (nextVine != -1 && !NextVine.active)
                nextVine = 1;

            float rotOff = (float)System.Math.Sin((Timer++ + (VineIndex * 12)) * 0.05f) * 0.2f;
            if (priorVine != -1)
                projectile.rotation = projectile.AngleTo(PriorVine.Center) - MathHelper.PiOver2 + rotOff;
            else if (nextVine != -1)
                projectile.rotation = projectile.AngleTo(NextVine.Center) - MathHelper.PiOver2 + rotOff;

            Rectangle playerTop = new Rectangle((int)p.position.X, (int)p.position.Y, p.width, 2);

            if (playerTop.Intersects(projectile.Hitbox) && (p.controlUp || p.controlDown) && !p.controlJump && !p.pulley && p.grappling[0] < 0 && !p.mount.Active && !Collision.SolidCollision(PulleyPosition(p), p.width, p.height) && projectile.timeLeft > 3)
            {
                p.pulley = true;
                p.pulleyDir = 1;
                p.position = projectile.position;
                p.fallStart = (int)(projectile.position.Y / 16f);

                p.GetModPlayer<VinePulleyPlayer>().currentVine = projectile.whoAmI;
            }
        }

        public Vector2 PulleyPosition(Player player)
        {
            float vineOff = player.GetModPlayer<VinePulleyPlayer>().vineOffset;
            float factor = 0;
            Vector2 otherPos = projectile.Center;

            if (nextVine != -1 && vineOff <= 0.5f)
            {
                otherPos = Vector2.Lerp(NextVine.Center, projectile.Center, 0.5f);
                factor = vineOff * 2f; 
            }
            else if (priorVine != -1 && vineOff > 0.5f)
            {
                otherPos = Vector2.Lerp(PriorVine.Center, projectile.Center, 0.5f);
                factor = 1 - ((vineOff - 0.5f) * 2f);
            }

            return Vector2.Lerp(otherPos, projectile.Center, factor) - new Vector2(player.width / 2f, -4);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; ++i)
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Grass, 0, 0);
        }
    }
}
