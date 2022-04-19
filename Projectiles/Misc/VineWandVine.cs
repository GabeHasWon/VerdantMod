using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Verdant.Projectiles.Misc
{
    public class VineWandVine : ModProjectile
    {
        public int nextVine = -1;
        public int priorVine = -1;

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
            projectile.width = 30;
            projectile.height = 38;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 2;
        }

        public override void AI()
        {
            Player p = Main.player[projectile.owner];
            p.heldProj = projectile.whoAmI;

            if (p.whoAmI != Main.myPlayer)
                return; //mp check (hopefully)

            projectile.timeLeft++;

            if (nextVine != -1)
                projectile.rotation = projectile.AngleTo(NextVine.Center) - MathHelper.PiOver2;

            Rectangle playerTop = new Rectangle((int)p.position.X, (int)p.position.Y, p.width, 2);

            if (playerTop.Intersects(projectile.Hitbox) && (p.controlUp || p.controlDown) && !p.controlJump && !p.pulley && p.grappling[0] < 0 && !p.mount.Active)
            {
                p.pulley = true;
                p.pulleyDir = 1;
                p.position = projectile.position;

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
    }
}
