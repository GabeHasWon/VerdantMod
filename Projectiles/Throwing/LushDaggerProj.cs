using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Projectiles.Throwing;

class LushDaggerProj : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.width = 18;
        Projectile.height = 18;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 120;
        Projectile.tileCollide = true;
        Projectile.aiStyle = 0;
    }

    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
        Projectile.velocity.Y += 0.1f;
    }

    public override void Kill(int timeLeft)
    {
        Gore.NewGorePerfect(Projectile.GetSource_Death(), Projectile.Center, new Vector2(0, 0), ModContent.GoreType<Gores.Verdant.LushLeaf>(), 1f);

        for (int i = 0; i < 3; ++i)
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BorealWood);
    }
}
