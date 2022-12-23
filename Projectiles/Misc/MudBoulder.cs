using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Projectiles.Misc
{
    public class MudBoulder : ModProjectile
    {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Lush Soil Ball");

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Boulder);
            Projectile.Size = new Vector2(12, 12);
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.hostile = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => oldVelocity.X != Projectile.velocity.X;

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; ++i)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Mud, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
        }
    }
}