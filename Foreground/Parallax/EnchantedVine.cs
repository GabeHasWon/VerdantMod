using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Verdant.Foreground.Parallax
{
    public class EnchantedVine : ParallaxedFGItem
    {
        public readonly Rectangle Hitbox;

        public int WhoAmI = 0;
        public bool active = false;
        public int owner = -1;
        public int frame = 0;
        public int lifeTimer = 0;

        public EnchantedVine NextVine = null;
        public EnchantedVine PriorVine = null;

        public bool perm = false;

        public EnchantedVine(Vector2 pos, int owner) : base(pos - new Vector2(58, 38) / 2f, Vector2.Zero, 1f, "Parallax/CloudbloomEntity")
        {
            this.owner = owner;

            lifeTimer = 10 * 60;
            WhoAmI = ForegroundManager.Items.Where(x => x is EnchantedVine).Count();
            source = new Rectangle(0, 0, 58, 38);
            velocity = new Vector2(0, Main.rand.NextFloat(0.25f, 0.75f) * (parallax * 1.2f)).RotatedByRandom(MathHelper.Pi);
            parallax = 1f;

            Hitbox = new Rectangle((int)Center.X - 4, (int)Center.Y, 10, 10);
        }

        public override void Update()
        {
            Player p = Main.player[owner];

            if (p.whoAmI != Main.myPlayer)
                return; //mp check (hopefully)

            if (PriorVine is not null && InvalidVine(true))
                PriorVine = null;

            if (NextVine is not null && InvalidVine(false))
                NextVine = null;

            if (!InvalidVine(true) && !InvalidVine(false))
                frame = 1;
            else
                frame = 0;

            float rotOff = (float)Math.Sin((lifeTimer-- + (WhoAmI * 12)) * 0.05f) * 0.2f;
            if (!InvalidVine(true))
                rotation = Vector2.Normalize(PriorVine.Center - Center).ToRotation() - MathHelper.PiOver2 + rotOff;
            else if (!InvalidVine(false))
                rotation = Vector2.Normalize(NextVine.Center - Center).ToRotation() - MathHelper.PiOver2 + rotOff;

            Rectangle playerTop = new((int)p.position.X, (int)p.position.Y, p.width, 2);

            if (playerTop.Intersects(Hitbox) && (p.controlUp || p.controlDown) && !p.controlJump && !p.pulley && p.grappling[0] < 0 && !p.mount.Active && !Collision.SolidCollision(p.position, p.width, p.height) && lifeTimer > 3)
            {
                p.pulley = true;
                p.pulleyDir = 1;
                p.position = Center;
                p.fallStart = (int)(position.Y / 16f);
                p.GetModPlayer<VinePulleyPlayer>().CurrentVine = this;
            }

            if (perm)
                lifeTimer = 10;

            if (lifeTimer < 0)
                Kill();

            //bool invalidNext = nextVine == -1 || InvalidVine(false);
            //bool invalidPrior = priorVine == -1 || InvalidVine(true);
            //if (invalidNext && invalidPrior && ForegroundManager.Items.Where(x => x is EnchantedVine).Count() <= 1)
            //    Kill();
        }

        private void Kill()
        {
            for (int i = 0; i < 3; ++i)
                Dust.NewDust(Center + Hitbox.Size() / 3f, Hitbox.Width / 2, Hitbox.Height / 2, DustID.Grass, 0, 0);
            killMe = true;
        }

        internal bool InvalidVine(bool prior)
        {
            EnchantedVine vine = prior ? PriorVine : NextVine;
            return vine is null || vine.killMe;
        }

        public void PulleyVelocity(Player player)
        {
            if (player.controlDown && player.controlUp)
                return;

            const float Speed = 4.75f;

            if (NextVine != null && player.controlUp)
                player.velocity = player.DirectionTo(NextVine.Center + new Vector2(0, 30)) * Speed;
            if (PriorVine != null && player.controlDown)
                player.velocity = player.DirectionTo(PriorVine.Center + new Vector2(0, 30)) * Speed;

            if (player.velocity.HasNaNs())
                player.velocity = Vector2.Zero;
        }

        public override void Draw()
        {
            int dir = (WhoAmI % 2) + (WhoAmI % 9) + (WhoAmI % 3); //"randomize" direction
            SpriteEffects effects = (dir % 2 == 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            var tex = VerdantMod.Instance.Assets.Request<Texture2D>("Foreground/Parallax/EnchantedVine").Value;

            drawColor = Lighting.GetColor(position.ToTileCoordinates());
            drawPosition = Center;

            if (!killMe)
                Main.spriteBatch.Draw(tex, drawPosition - Main.screenPosition, new Rectangle(0, frame * 20, 20, 20), drawColor, rotation, new Vector2(10), 1f, effects, 0);
            //base.Draw();
        }
    }
}