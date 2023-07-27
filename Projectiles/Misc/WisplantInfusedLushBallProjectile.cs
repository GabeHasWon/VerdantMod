using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems;

namespace Verdant.Projectiles.Misc
{
    public class WisplantInfusedLushBallProjectile : MudBoulder
    {
        public override string Texture => base.Texture.Replace(nameof(WisplantInfusedLushBallProjectile), nameof(MudBoulder));

        private static Asset<Texture2D> _extraTextures;

        private bool[] _downPlants = new bool[3];

        public override void Unload() => _extraTextures = null;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wisplant-Infused Lush Soil Ball");
            _extraTextures = ModContent.Request<Texture2D>("Verdant/Projectiles/Misc/WisplantInfusedMudBoulderProps");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Boulder);
            Projectile.Size = new Vector2(12, 12);
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.hostile = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => oldVelocity.X != Projectile.velocity.X;

        public override void AI()
        {
            if (!_downPlants[0] && Collision.SolidCollision(Projectile.Center - new Vector2(6, 0).RotatedBy(Projectile.rotation), 4, 4))
                _downPlants[0] = true;

            if (!_downPlants[1] && Collision.SolidCollision(Projectile.Center - new Vector2(2, 6).RotatedBy(Projectile.rotation), 6, 6))
                _downPlants[1] = true;

            if (!_downPlants[2] && Collision.SolidCollision(Projectile.Center - new Vector2(0, 4).RotatedBy(Projectile.rotation), 6, 6))
                _downPlants[2] = true;

            RandomUpdating.Auto((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f) + 1, false, 3, (i, j) =>
            {
                Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.TerraBlade, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            });

            if (Main.rand.NextBool(20))
                Dust.NewDust(Projectile.position, 16, 16, DustID.TerraBlade, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-3f, -1f));
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; ++i)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Mud, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));

            for (int i = 0; i < 6; ++i)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));

            for (int i = 0; i < 2; ++i)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PinkStarfish, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = _extraTextures.Value;
            Vector2 origin = new(9, 11);

            for (int i = 0; i < 3; ++i)
            {
                Rectangle source = new Rectangle(i * 20, _downPlants[i] ? 20 : 0, 18, 18);
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, source, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }
        }
    }
}