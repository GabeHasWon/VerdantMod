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

        public ref Player Owner => ref Main.player[Projectile.owner];

        private ref float PauseTimer => ref Projectile.ai[0];

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
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.width = 30;
            Projectile.height = 38;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override bool? CanCutTiles() => false;

        public override void AI()
        {
            Owner.heldProj = Projectile.whoAmI;

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
                Projectile.Center = Vector2.Lerp(Projectile.Center, Owner.Center, 0.3f);

                if (Projectile.DistanceSQ(Owner.Center) < 10 * 10)
                    Projectile.Kill();
            }
        }

        private void Animate()
        {
            if (++Projectile.frameCounter > 4)
            {
                if (++Projectile.frame > 2)
                    Projectile.frame = 0;
                Projectile.frameCounter = 0;
            }
        }

        private void Movement()
        {
            if (!_rightChannel && PauseTimer-- <= 0)
                Projectile.Center = Vector2.Lerp(Projectile.Center, TargetPosition, 0.025f);

            if (_rightChannel && Main.mouseRightRelease)
                LetGo();
            if (!_rightChannel && Main.mouseRight)
                _rightChannel = true;
        }

        private void LetGo()
        {
            _rightChannel = false;

            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld) * 15, ProjectileID.Bullet, Projectile.damage, 0f, Projectile.owner);
            PauseTimer = 20;
        }

        public override void PostDraw(Color lightColor)
        {
            if (_rightChannel)
                DrawTarget();
        }

        private void DrawTarget()
        {
            Texture2D targetTex = Mod.Assets.Request<Texture2D>("Projectiles/Misc/VerdantWispTargetting").Value;
            Vector2 initialDrawPos = Projectile.Center - Main.screenPosition;
            Vector2 direction = (Main.MouseWorld - Projectile.Center) * 0.75f;

            for (int i = 0; i < 8; ++i)
            {
                Vector2 offset = direction * i;
                Rectangle src = new Rectangle(0, 0, 30, 30);


                Main.EntitySpriteDraw(targetTex, initialDrawPos + offset, src, Color.White, 0f, targetTex.Size() / 2f, 1f, SpriteEffects.None, 0);
            }
        }
    }
}