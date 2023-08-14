using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Armour.ApotheoticArmor;

internal class FruitProjectile : ModProjectile
{
    private ref float TargettedMinionWhoAmI => ref Projectile.ai[0];

    private FruitType Fruit
    {
        get => (FruitType)Projectile.ai[1];
        set => Projectile.ai[1] = (float)value;
    }

    private ref float Timer => ref Projectile.ai[2];

    private Projectile TargettedMinion => Main.projectile[(int)TargettedMinionWhoAmI];

    public override void SetDefaults()
    {
        Projectile.aiStyle = -1;
        Projectile.width = Projectile.height = 14;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.extraUpdates = 1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 7;
        Projectile.timeLeft = 2;

        AIType = 0;
    }

    public override void AI()
    {
        Timer++;

        Projectile.timeLeft++;
        Projectile.Center = Vector2.Lerp(Projectile.Center, TargettedMinion.Center, Timer * 0.005f);

        if (TargettedMinion.DistanceSQ(Projectile.Center) < MathF.Pow(Projectile.width * 0.8f, 2))
        {
            TreeFruitProjectile proj = TargettedMinion.GetGlobalProjectile<TreeFruitProjectile>();
            proj.fruitBuff = Fruit;
            proj.fruitTime = TreeFruitProjectile.MaxFruitTime;

            TargettedMinion.netUpdate = true;
            Projectile.Kill();
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var tex = TextureAssets.Projectile[Type].Value;
        var frame = new Rectangle(12 * (int)(Fruit - 1), 0, 10, 14);
        var pos = Projectile.Center - Main.screenPosition;

        Main.EntitySpriteDraw(tex, pos, frame, lightColor, Projectile.velocity.X * 0.8f, frame.Size() / 2f, 1f, SpriteEffects.None);
        return false;
    }
}