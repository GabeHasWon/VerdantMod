using System;
using Terraria;
using Terraria.ModLoader;

namespace Verdant.Projectiles.Misc;

class FlotieOfWrath : ModProjectile
{
    Player Owner => Main.player[Projectile.owner];

    private ref float Timer => ref Projectile.ai[0];

    public override void SetStaticDefaults() => Main.projFrames[Type] = 2;

    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.width = 50;
        Projectile.height = 64;
        Projectile.timeLeft = 300;
        Projectile.tileCollide = false;
        Projectile.aiStyle = 0;
    }

    public override bool? CanCutTiles() => false;

    public override void AI()
    {
        Owner.GetModPlayer<Buffs.Pet.PetPlayer>().PetFlag(Projectile);

        if (Projectile.DistanceSQ(Owner.Center) > 300 * 300)
        {
            Projectile.velocity += (Owner.Center - Projectile.Center) * 0.001f;

            if (Projectile.velocity.LengthSquared() > 8 * 8)
                Projectile.velocity = Projectile.velocity.SafeNormalize(Microsoft.Xna.Framework.Vector2.Zero) * 8;
        }
        else
            Projectile.velocity *= 0.98f;

        Projectile.rotation = Projectile.velocity.X * 0.05f;
        Projectile.velocity.Y += MathF.Sin(Timer++ * 0.04f) * 0.05f;
        Projectile.frame = Projectile.velocity.Y <= 0 ? 0 : 1;

        if (Math.Abs(Projectile.velocity.X) > 0.0008f)
            Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);

        Lighting.AddLight(Projectile.position, new Microsoft.Xna.Framework.Vector3(0.5f, 0.16f, 0.30f) * 2f);
    }
}
