using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;
using Verdant.Drawing;
using Verdant.NPCs.Passive.Floties;

namespace Verdant.Projectiles.Misc;

class FlotieOfWrath : ModProjectile, IDrawAdditive
{
    private static Asset<Texture2D> _shineTex;

    Player Owner => Main.player[Projectile.owner];

    private ref float Timer => ref Projectile.ai[0];
    private ref float State => ref Projectile.ai[1];

    private ref float FadeIn => ref Projectile.localAI[0];

    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 2;

        _shineTex = ModContent.Request<Texture2D>(Texture + "_Shine");
    }

    public override void Unload() => _shineTex = null;

    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.width = 38;
        Projectile.height = 54;
        Projectile.timeLeft = 300;
        Projectile.tileCollide = false;
        Projectile.aiStyle = 0;
    }

    public override bool? CanCutTiles() => false;

    public override void AI()
    {
        Owner.GetModPlayer<Buffs.Pet.PetPlayer>().PetFlag(Projectile);

        var center = GetTarget(Projectile.DistanceSQ(Owner.Center), out bool flotie);
        float dist = Projectile.DistanceSQ(center);

        if (State == 0)
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 5;

            if (dist > 200 * 200 || flotie)
            {
                if (dist > 1000 * 1000)
                    State = 1;

                Projectile.velocity += (center - Projectile.Center) * 0.0005f;

                float maxSpeed = 8 * 8;

                if (Projectile.velocity.LengthSquared() > maxSpeed)
                    Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 8;

                if (flotie && dist > 100 * 100)
                    Projectile.velocity *= 0.99f;
            }
            else
                Projectile.velocity *= 0.98f;
        }
        else
        {
            Projectile.alpha = (int)(FadeIn++ / 20f * 255);

            if (FadeIn >= 20)
            {
                State = 0;
                FadeIn = 0;
                Projectile.Center = Owner.Center;
            }
        }

        Projectile.rotation = Projectile.velocity.X * 0.05f;
        Projectile.velocity.Y += MathF.Sin(Timer++ * 0.04f) * 0.05f;
        Projectile.frame = Projectile.velocity.Y <= 0 ? 0 : 1;

        if (Math.Abs(Projectile.velocity.X) > 0.0008f)
            Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);

        Lighting.AddLight(Projectile.position, new Vector3(0.5f, 0.16f, 0.30f) * 2f);
    }

    private Vector2 GetTarget(float distFromPlayer, out bool flotie)
    {
        flotie = false;

        if (distFromPlayer > 800 * 800)
            return Owner.Center;

        foreach (var npc in ActiveEntities.NPCs)
        {
            float npcDist = npc.DistanceSQ(Projectile.Center);

            if (npcDist < 400 * 400 && npc.type == ModContent.NPCType<Flotie>() && Math.Sqrt(npcDist) + Math.Sqrt(distFromPlayer) < 800)
            {
                flotie = true;
                return npc.Center;
            }
        }
        return Owner.Center;
    }

    public void DrawAdditive(AdditiveLayer layer)
    {
        if (layer == AdditiveLayer.AfterPlayer)
            return;

        int frameY = _shineTex.Value.Height / Main.projFrames[Type] * Projectile.frame;
        var source = new Rectangle(0, frameY, Projectile.width, Projectile.height);
        var col = Color.LightYellow * 0.65f;
        Main.spriteBatch.Draw(_shineTex.Value, Projectile.Center - Main.screenPosition, source, col, Projectile.rotation, Projectile.Size / 2f, 1f, SpriteEffects.None, 0);
    }
}
