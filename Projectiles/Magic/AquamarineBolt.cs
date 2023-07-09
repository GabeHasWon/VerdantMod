using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Drawing;

namespace Verdant.Projectiles.Magic;

class AquamarineBolt : ModProjectile, IDrawAdditive
{
    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.width = 12;
        Projectile.height = 12;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 60 * 5;
        Projectile.tileCollide = false;
        Projectile.aiStyle = 0;
    }

    public override void AI()
    {
        for (int i = 0; i < 2; ++i)
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale = Main.rand.NextFloat(0.6f, 2f);
        }
    }

    public void DrawAdditive(AdditiveLayer layer)
    {
        if (layer == AdditiveLayer.BeforePlayer)
            return;
        
        Texture2D tex = Mod.Assets.Request<Texture2D>("Textures/Circle").Value;
        float rot = Projectile.velocity.ToRotation();
        Vector2 scale = new Vector2(1 + (Projectile.velocity.Length() * 0.05f), 1) * 0.5f;
        float sc = 0.9f - (float)(Math.Sin(Projectile.timeLeft * 0.03f) * 0.05f);
        var col = new Color(138, 185, 189);
        var orig = new Vector2(tex.Width * 0.6f, tex.Height / 2f);

        Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, col * 0.25f, rot, orig, scale * sc, SpriteEffects.None, 1f);
        Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, col * 0.45f, rot, orig, scale * sc * 0.5f, SpriteEffects.None, 1f);
    }
}
