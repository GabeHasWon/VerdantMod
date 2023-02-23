using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using Verdant.Drawing;
using Verdant.Projectiles.Particles;
using Verdant.Tiles;

namespace Verdant.Projectiles.Magic
{
    class HealPlants : ModProjectile, IDrawAdditive
    {
        static Asset<Texture2D> _additiveTex;

        public override void SetStaticDefaults() => _additiveTex = ModContent.Request<Texture2D>(Texture + "_Additive");

        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.aiStyle = 0;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(5);
            Projectile.rotation = Main.rand.NextFloat(-0.1f, 0.1f);
            Projectile.scale = Main.rand.NextFloat(0.9f, 1.1f);
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 60)
                Projectile.timeLeft++;

            foreach (var player in ActiveEntities.Players)
            {
                if (player.Hitbox.Intersects(Projectile.Hitbox))
                {
                    Projectile.Kill();
                    player.Heal(3);

                    for (int j = 0; j < 3; ++j)
                    {
                        Vector2 particleVel = new Vector2(Main.rand.NextFloat(4, 12), 0).RotatedByRandom(MathHelper.TwoPi);
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, particleVel, ModContent.ProjectileType<HealingParticle>(), 0, 0, Projectile.owner, 0, 0);
                    }
                    return;
                }
            }

            var center = Projectile.Center.ToTileCoordinates();
            Vector2 discard = default;
            Projectile.rotation = ModContent.GetInstance<TileSwaySystem>().GetGrassSway(center.X, center.Y, ref discard);
            Lighting.AddLight(Projectile.Center, new Vector3(0.1f, 0.5f, 0.2f) * 0.5f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 pos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY + 14);
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            float fadeIn = 1 - Math.Max((Projectile.timeLeft - 60) / 60f, 0f);
            var effect = Projectile.whoAmI % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.spriteBatch.Draw(tex, pos, new Rectangle(24 * Projectile.frame, 0, 22, 46), Color.Lerp(lightColor, Color.White, 0.25f) * fadeIn, Projectile.rotation, new Vector2(11, 46), 1f, effect, 0f);
            return false;
        }

        public void DrawAdditive(AdditiveLayer layer)
        {
            if (layer == AdditiveLayer.AfterPlayer)
                return;

            float fadeIn = 1 - Math.Max((Projectile.timeLeft - 60) / 60f, 0f);
            Vector2 pos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
            var source = new Rectangle(52 * Projectile.frame, 0, 50, 74);
            var effect = Projectile.whoAmI % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.spriteBatch.Draw(_additiveTex.Value, pos, source, Color.White * 0.35f * fadeIn, Projectile.rotation, new Vector2(25, 46), 1f, effect, 0);
        }
    }
}
