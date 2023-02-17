using Terraria;
using Terraria.ModLoader;

namespace Verdant.Projectiles.Misc;

class FlotieOfWrath : ModProjectile
{
    Player Owner => Main.player[Projectile.owner];

    public override void SetStaticDefaults() => Main.projFrames[Type] = 1;

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

        Projectile.rotation = Projectile.velocity.X * 0.05f;

        if (Projectile.DistanceSQ(Owner.Center) > 100 * 100)
        {
            Projectile.velocity += (Owner.Center - Projectile.Center) * 0.001f;

            if (Projectile.velocity.LengthSquared() > 8 * 8)
                Projectile.velocity = Projectile.velocity.SafeNormalize(Microsoft.Xna.Framework.Vector2.Zero) * 8;
        }
        else
            Projectile.velocity *= 0.98f;

        Lighting.AddLight(Projectile.position, new Microsoft.Xna.Framework.Vector3(0.5f, 0.16f, 0.30f) * 1.4f);
    }
}
