using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Projectiles.Misc;

internal class VineHookProjectile : ModProjectile
{
    public static Asset<Texture2D> _chain;

    public override void Unload() => _chain = null;

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");

        _chain = ModContent.Request<Texture2D>(Texture + "_Chain");
    }

    public override void AI()
    {
        var hitBox = Projectile.Hitbox;
        hitBox.Inflate(4, 4);

        if (Projectile.ai[0] == 2 && hitBox.Intersects(Main.player[Projectile.owner].Hitbox))
        {
            Projectile.Kill();

            if (Main.rand.NextBool(2))
            {
                string[] choices = new string[] { "Woo!", "Yahoo!", "Wee!" };
                int c = CombatText.NewText(Projectile.Hitbox, Color.LawnGreen, Main.rand.Next(choices));
                var combat = Main.combatText[c];

                combat.rotation = Main.rand.NextFloat(-0.2f, 0.2f);
                combat.scale *= Main.rand.NextFloat(0.85f, 1.1f);
            }
        }
    }

    public override void SetDefaults() => Projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
    public override bool? CanUseGrapple(Player player) => player.ownedProjectileCounts[Type] < 3;
    public override float GrappleRange() => 16 * 30;
    public override void NumGrappleHooks(Player player, ref int numHooks) => numHooks = 1;
    public override void GrappleRetreatSpeed(Player player, ref float speed) => speed = 20;
    public override void GrapplePullSpeed(Player player, ref float speed) => speed = 15;

    public override void PostDraw(Color lightColor)
    {
        Vector2 center = Projectile.Center;
        Vector2 offset = Main.player[Projectile.owner].MountedCenter - center;

        if (Projectile.Center.HasNaNs() || offset.HasNaNs())
            return;

        int realHeight = (_chain.Height() / 2) - 2;

        while (true)
        {
            if (offset.Length() < realHeight + 1)
                return;
            else
            {
                center += Vector2.Normalize(offset) * realHeight;
                offset = Main.player[Projectile.owner].MountedCenter - center;

                Color color = Projectile.GetAlpha(Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16.0)));
                Rectangle source = new(0, offset.Length() < realHeight * 1.2f + 1 ? 20 : 0, 18, 18);

                Main.spriteBatch.Draw(_chain.Value, center - Main.screenPosition, source, color, offset.ToRotation() - 1.57f, _chain.Size() / 2, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}