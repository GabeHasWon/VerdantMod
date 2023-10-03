using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems;

namespace Verdant.Projectiles.Misc;

class MagicWisplantProj : ModProjectile
{
    public override void SetStaticDefaults() => Main.projFrames[Type] = 1;

    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.width = 22;
        Projectile.height = 22;
        Projectile.timeLeft = 300;
        Projectile.tileCollide = false;
        Projectile.aiStyle = 0;
    }

    public override bool? CanCutTiles() => false;

    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.X * 0.2f;
        Projectile.velocity *= 0.96f;

        RandomUpdating.CircularUpdate((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), 10, 15, (i, j) => 
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.TerraBlade, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
        });

        if (Projectile.timeLeft < 30)
            Projectile.alpha = Math.Min(255 - (int)(255 * ((Projectile.timeLeft + 30) / 60f)), 125);
    }

    public override void OnKill(int timeLeft)
    {
        for (int i = 0; i < 6; ++i)
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));

        for (int i = 0; i < 2; ++i)
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PinkStarfish, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
    }
}
