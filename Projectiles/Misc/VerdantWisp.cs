using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Projectiles.Misc
{
    class VerdantWisp : ModProjectile
    {
        public const int MaxDistance = 600;

        public ref Player Owner => ref Main.player[projectile.owner];

        private ref float PauseTimer => ref projectile.ai[0];

        private Vector2 TargetPosition
        {
            get
            {
                if (Owner.DistanceSQ(Main.MouseWorld) < MaxDistance * MaxDistance)
                    return Main.MouseWorld;
                return Owner.Center + (Owner.DirectionTo(Main.MouseWorld) * MaxDistance);
            }
        }

        private bool _rightChannel = false;

        private bool _killMe = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Memory");
            Main.projFrames[projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.width = 30;
            projectile.height = 38;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.magic = true;
        }

        public override bool? CanCutTiles() => false;

        public override void AI()
        {
            Owner.heldProj = projectile.whoAmI;

            if (Owner.whoAmI != Main.myPlayer)
                return; //mp check (hopefully)

            CheckAlive();
            Animate();
            if (!_killMe)
                Movement();
        }

        private void CheckAlive()
        {
            if (!Owner.channel)
                _killMe = true;

            if (_killMe)
            {
                projectile.Center = Vector2.Lerp(projectile.Center, Owner.Center, 0.3f);

                if (projectile.DistanceSQ(Owner.Center) < 10 * 10)
                    projectile.Kill();
            }
        }

        private void Animate()
        {
            if (++projectile.frameCounter > 4)
            {
                if (++projectile.frame > 2)
                    projectile.frame = 0;
                projectile.frameCounter = 0;
            }
        }

        private void Movement()
        {
            if (!_rightChannel && PauseTimer-- <= 0)
                projectile.Center = Vector2.Lerp(projectile.Center, TargetPosition, 0.025f);

            if (_rightChannel && Main.mouseRightRelease)
                LetGo();
            if (!_rightChannel && Main.mouseRight)
                _rightChannel = true;
        }

        private void LetGo()
        {
            _rightChannel = false;

            Projectile.NewProjectile(projectile.Center, projectile.DirectionTo(Main.MouseWorld) * 15, ProjectileID.Bullet, projectile.damage, 0f, projectile.owner);
            PauseTimer = 20;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (_rightChannel)
                DrawTarget(spriteBatch);
        }

        private void DrawTarget(SpriteBatch b)
        {
            Texture2D targetTex = mod.GetTexture("Projectiles/Misc/VerdantWispTargetting");
            Vector2 initialDrawPos = projectile.Center - Main.screenPosition;
            Vector2 direction = (Main.MouseWorld - projectile.Center) * 0.75f;

            for (int i = 0; i < 8; ++i)
            {
                Vector2 offset = direction * i;
                Rectangle src = new Rectangle(0, 0, 30, 30);


                b.Draw(targetTex, initialDrawPos + offset, src, Color.White, 0f, targetTex.Size() / 2f, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}