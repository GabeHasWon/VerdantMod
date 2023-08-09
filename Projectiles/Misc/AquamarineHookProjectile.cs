using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Projectiles.Misc;

internal class AquamarineHookProjectile : ModProjectile
{
    public static Asset<Texture2D> _chain;

    public override void SetStaticDefaults() => _chain = ModContent.Request<Texture2D>(Texture + "_Chain");
    public override void Unload() => _chain = null;
    public override void SetDefaults() => Projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
    public override bool? CanUseGrapple(Player player) => player.ownedProjectileCounts[Type] < 2;
    public override float GrappleRange() => 16 * 27;
    public override void NumGrappleHooks(Player player, ref int numHooks) => numHooks = 1;
    public override void GrappleRetreatSpeed(Player player, ref float speed) => speed = 16;
    public override void GrapplePullSpeed(Player player, ref float speed) => speed = 12.25f;

    public override void PostDraw(Color lightColor)
    {
        Vector2 center = Projectile.Center;
        Vector2 offset = Main.player[Projectile.owner].MountedCenter - center;

        if (Projectile.Center.HasNaNs() || offset.HasNaNs())
            return;

        int realHeight = _chain.Height();

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