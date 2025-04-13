using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Buffs.Minion;
using Verdant.Drawing;
using Verdant.Projectiles.Particles;

namespace Verdant.Projectiles.Minion;

class VerdantHealingMinion : ModProjectile, IDrawAdditive
{
    ref float State => ref Projectile.ai[0];
    ref float Timer => ref Projectile.ai[1];
    ref float HealingTimer => ref Projectile.ai[2];

    public Vector2 goPosition = -Vector2.One;

    public override void SetStaticDefaults()
    {
        Main.projPet[Type] = true;
        ProjectileID.Sets.TrailCacheLength[Type] = 3;
        ProjectileID.Sets.TrailingMode[Type] = 0;
        ProjectileID.Sets.MinionSacrificable[Type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.width = 48;
        Projectile.height = 48;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.minionSlots = 1f;
        Projectile.minion = true;
    }

    public override bool? CanCutTiles() => false;
    public override bool MinionContactDamage() => false;

    public override void AI()
    {
        Timer++;
        HealingTimer++;

        float sc = 0.5f - (float)(Math.Sin(Timer * 0.03f) * 0.1f);

        if (Projectile.velocity.Length() < 3f)
            Lighting.AddLight(Projectile.Center, new Vector3(0.4f, 0.12f, 0.24f) * 6 * ((3f - Projectile.velocity.Length()) / 3) * sc);

        for (int i = 0; i < Main.player.Length; ++i)
        {
            Player player = Main.player[i];
            Vector2 off = new(0, (float)(Math.Sin(Timer * 0.03f) * 6));
            float rad = .9f - ((float)(Math.Sin(Timer * 0.03f) * 0.05f));
            float radMult = 1f;

            if (State == 1) 
                radMult = 1.2f;

            bool isClose = Vector2.DistanceSquared(player.MountedCenter, Projectile.Center - off) < MathF.Pow(rad * radMult * 196, 2);

            if (HealingTimer >= 120 && player.active && !player.dead && isClose)
            {
                if (player.statLife < player.statLifeMax2 - 10)
                {
                    player.HealEffect(10, true);
                    player.statLife += 10;

                    for (int j = 0; j < 3; ++j)
                    {
                        Vector2 particleVel = new Vector2(Main.rand.NextFloat(4, 12), 0).RotatedByRandom(MathHelper.TwoPi);
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, particleVel, ModContent.ProjectileType<HealingParticle>(), 0, 0, Projectile.owner, 0, 0);
                    }
                }

            }
        }

        if (HealingTimer >= 120)
            HealingTimer = 0;

        State = 0;

        if (Main.myPlayer == Projectile.owner && !Main.player[Projectile.owner].HasBuff<HealingFlowerBuff>())
            Projectile.Kill();

        if (Projectile.frameCounter++ % 30 == 0)
            Projectile.frame = Projectile.frame == 0 ? 1 : 0;

        if (goPosition != -Vector2.One)
        {
            Projectile.velocity = (goPosition - Projectile.position) * 0.05f;

            if (Vector2.Distance(Projectile.position, goPosition) < 10)
                goPosition = -Vector2.One;
        }

        Projectile.velocity *= 0.85f;
        Projectile.timeLeft = 2;
    }

    public override void SendExtraAI(BinaryWriter writer) => writer.WriteVector2(goPosition);
    public override void ReceiveExtraAI(BinaryReader reader) => goPosition = reader.ReadVector2();

    public override bool PreDraw(ref Color lightColor)
    {
        //Summon anim
        float scale = 1;

        //General function
        Texture2D proj = ModContent.Request<Texture2D>("Verdant/Projectiles/Minion/VerdantHealingMinion").Value;
        if (State <= 1) //Normal minion
        {
            scale *= State == 0 ? 1f : 1.2f; //Larger when stacked

            DrawCircle(lightColor, scale, out Vector2 off);

            if (goPosition != -Vector2.One) //Indicator for move position
                Main.EntitySpriteDraw(proj, goPosition - Main.screenPosition - off + new Vector2(24), proj.Frame(2, 1, Projectile.frame), Color.White * 0.2f, 0f, new Vector2(24), scale, SpriteEffects.None, 1);

            for (int k = Projectile.oldPos.Length - 1; k >= 0; k--)
            {
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / Projectile.oldPos.Length);
                Main.EntitySpriteDraw(proj, Projectile.oldPos[k] - Main.screenPosition - off + new Vector2(24), proj.Frame(2, 1, Projectile.frame), color, 0f, new Vector2(24), scale, SpriteEffects.None, 1);
            }
        }
        return false;
    }

    private void DrawCircle(Color lightColor, float scale, out Vector2 off)
    {
        Texture2D circle = ModContent.Request<Texture2D>("Verdant/Projectiles/Minion/VerdantHealingCircle").Value;
        float sc = 1.9f - (float)(Math.Sin(Timer * 0.03f) * 0.05f);
        float alphaScale = (sc - 1.78f) * 8;
        Color circleCol = Projectile.GetAlpha(lightColor) * alphaScale;

        off = new Vector2(0, (float)(Math.Sin(Timer * 0.03f) * 6));

        Main.EntitySpriteDraw(circle, Projectile.Center - Main.screenPosition - off, circle.Frame(), circleCol, Timer * 0.006f, circle.Bounds.Center.ToVector2(), sc * scale, SpriteEffects.None, 1);
        if (alphaScale > 0.95f)
        {
            Color col = Projectile.GetAlpha(lightColor) * ((alphaScale - 0.95f) * 1.5f);

            Main.EntitySpriteDraw(circle, Projectile.Center - Main.screenPosition - off, circle.Frame(), col, Timer * 0.006f, circle.Bounds.Center.ToVector2(), sc * scale * 1.04f, SpriteEffects.None, 1);
            Main.EntitySpriteDraw(circle, Projectile.Center - Main.screenPosition - off, circle.Frame(), col, Timer * 0.006f, circle.Bounds.Center.ToVector2(), sc * scale * 0.96f, SpriteEffects.None, 1);
        }
    }

    public void DrawAdditive(AdditiveLayer layer)
    {
        if (layer == AdditiveLayer.AfterPlayer)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Textures/Circle").Value;
            float rot = Projectile.velocity.ToRotation();
            Vector2 scale = new Vector2(1 + (Projectile.velocity.Length() * 0.02f), 1) * 5f;
            float sc = 0.9f - (float)(Math.Sin(Timer * 0.03f) * 0.05f);

            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, new Color(0, 170, 62) * 0.45f, rot, tex.Size() / 2f, scale * sc, SpriteEffects.None, 1f);
        }
    }
}
