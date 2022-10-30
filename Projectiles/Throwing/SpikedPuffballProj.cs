using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Projectiles.Throwing;

class SpikedPuffballProj : ModProjectile
{
    private const int UnpuffThreshold = 30 * 60;

    private ref float Timer => ref Projectile.ai[0];

    public override void SetStaticDefaults() => Main.projFrames[Type] = 2;

    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.width = 22;
        Projectile.height = 22;
        Projectile.penetrate = 5;
        Projectile.timeLeft = 600 * 60;
        Projectile.tileCollide = true;
        Projectile.aiStyle = 0;
    }

    public override void AI()
    {
        Projectile.rotation += 0.01f * Projectile.velocity.X * (Timer > UnpuffThreshold ? 4 : 2);
        Projectile.velocity *= 0.985f;

        if (Timer == UnpuffThreshold)
            for (int i = 0; i < 4; ++i)
                Dust.NewDust(Projectile.position, 24, 24, DustID.PinkStarfish);

        if (Timer++ > UnpuffThreshold)
        {
            Projectile.frame = 1;
            Projectile.velocity.Y += 0.2f;
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (Timer < UnpuffThreshold)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10 with { Volume = 0.3f }, Projectile.position);

            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                Projectile.velocity.X = -oldVelocity.X;

            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                Projectile.velocity.Y = -oldVelocity.Y;
        }
        return false;
    }
}
