using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Verdant.Drawing;

namespace Verdant.Projectiles.Particles;

class ApotheosisHand : ModProjectile, IDrawAdditive
{
    private static Asset<Texture2D> _wingTex;

    public ref float Timer => ref Projectile.ai[0];

    private Color drawCol = Color.Green;

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Apotheosis Hand");
        _wingTex = ModContent.Request<Texture2D>(Texture + "_Wings");
    }

    public override void Unload() => _wingTex = null;

    public override void SetDefaults()
    {
        Projectile.hostile = false;
        Projectile.friendly = false;
        Projectile.width = 30;
        Projectile.height = 38;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.timeLeft = 2000;
        Projectile.alpha = 255;

        drawCol = Color.White;// new Color(Main.rand.NextFloat(0.8f, 0.85f), Main.rand.NextFloat(0.8f, 0.85f), Main.rand.NextFloat(0.9f, 1f));
    }

    public override void AI()
    {
        Projectile.velocity *= 0.93f;
        Projectile.alpha -= 3;

        if (Projectile.alpha <= 0)
            Projectile.Kill();
    }

    public override bool PreDraw(ref Color lightColor) => false;

    public void DrawAdditive(AdditiveLayer layer)
    {
        if (layer == AdditiveLayer.AfterPlayer)
            return;

        Color col = drawCol * (Projectile.alpha / 255f);
        Vector2 drawPos = Projectile.Center - Main.screenPosition;
        Texture2D tex = TextureAssets.Projectile[Type].Value;
        Main.spriteBatch.Draw(tex, drawPos, null, col, 0f, tex.Size() / 2f, 1f, SpriteEffects.None, 1f);
        Main.spriteBatch.Draw(tex, drawPos, null, col * 0.5f, 0f, tex.Size() / 2f, 1.5f, SpriteEffects.None, 1f);
        Main.spriteBatch.Draw(tex, drawPos, null, col * 0.25f, 0f, tex.Size() / 2f, 2f, SpriteEffects.None, 1f);

        col = Color.SkyBlue * (Projectile.alpha / 255f);
        tex = _wingTex.Value;
        float rot = (-MathHelper.PiOver4 * Projectile.velocity.Y * 0.2f) + MathHelper.PiOver4;

        for (int i = 0; i < 4; ++i)
        {
            Main.spriteBatch.Draw(tex, drawPos - new Vector2(10, 0), null, col * (1f / i), -rot + MathHelper.PiOver4, tex.Size(), 1 + ((i - 1) * 0.25f), SpriteEffects.None, 1f);
            Main.spriteBatch.Draw(tex, drawPos + new Vector2(12, 0), null, col * (1f / i), rot - MathHelper.PiOver4, new(0, tex.Height), 1 + ((i - 1) * 0.25f), SpriteEffects.FlipHorizontally, 1f);
        }
    }
}
