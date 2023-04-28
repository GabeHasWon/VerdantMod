using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Dusts;

namespace Verdant.Systems.Foreground.Parallax
{
    public class CloudbloomEntity : ParallaxedFGItem
    {
        const int Width = 58;

        private int DustType => puff ? ModContent.DustType<PuffDust>() : DustID.Cloud;

        private bool BouncedUpon = false;
        private int bounceTimer = 0;
        private int rotTimer = 0;
        internal Vector2 anchor = Vector2.Zero;
        internal bool puff = false;

        public CloudbloomEntity(Vector2 pos, bool isPuff = false) : base(pos - new Vector2(58, 38) / 2f, Vector2.Zero, 1f, "Parallax/CloudbloomEntity")
        {
            source = new Rectangle(0, 0, 58, 38);
            velocity = new Vector2(0, Main.rand.NextFloat(0.25f, 0.75f) * (parallax * 1.2f)).RotatedByRandom(MathHelper.Pi);
            anchor = Center;
            parallax = 1f;
            puff = isPuff;
            Texture = VerdantMod.Instance.Assets.Request<Texture2D>("Systems/Foreground/Parallax/CloudbloomEntity" + (puff ? "Puff" : ""));
        }

        public override void Update()
        {
            base.Update();

            velocity *= 0.9f;
            
            if (Vector2.DistanceSquared(Center, anchor) < 2 * 2)
                velocity *= 0.96f;
            else
                velocity += (Center == anchor) ? Vector2.Zero : Vector2.Normalize(anchor - Center) * 0.5f;

            bounceTimer++;
            rotation = (float)Math.Sin(rotTimer++ * 0.05f) * 0.28f;

            if (Main.rand.NextBool(puff ? 30 : 14))
                Dust.NewDustPerfect(new Vector2(position.X + 4 + Main.rand.Next(Width - 8), Center.Y), DustType, new Vector2(0, Main.rand.NextFloat(0.2f, 0.5f)));

            Rectangle top = new((int)position.X, (int)position.Y, Width, 8);

            if (!BouncedUpon)
            {
                for (int i = 0; i < Main.maxPlayers; ++i)
                {
                    Player p = Main.player[i];
                    
                    if (p.active && !p.dead)
                    {
                        Rectangle pBottom = new((int)p.position.X, (int)p.Bottom.Y, p.width, 4);
                        if (top.Intersects(pBottom) && p.velocity.Y > 0 && BounceOnPlayer(p))
                            break;
                    }
                }
            }
            else
            {
                scale = (bounceTimer / 60f) * 0.25f + 0.75f;

                if (bounceTimer > 20)
                    BouncedUpon = false;
            }
        }

        private bool BounceOnPlayer(Player p)
        {
            void SpawnDust()
            {
                for (int j = 0; j < 14; ++j)
                {
                    var spawnPos = position + new Vector2(4, 12);
                    if (puff)
                        spawnPos += new Vector2(0, Main.rand.Next(-10, 14));

                    Dust.NewDust(spawnPos, Width - 8, 8, DustType, Main.rand.NextFloat(-0.2f, 0.2f) + velocity.X, Main.rand.NextFloat(2, 3f));
                }
            }

            if (!p.controlDown) //Jump up
            {
                velocity = new Vector2(p.velocity.X * 0.2f, 4 + p.velocity.Y * 0.5f);
                p.velocity.Y = -14;
                p.velocity.X *= 1.2f;
                p.fallStart = (int)(position.Y / 16f);

                BouncedUpon = true;
                bounceTimer = 0;

                SpawnDust();

                SoundEngine.PlaySound(new SoundStyle("Verdant/Sounds/CloudJump") with { PitchVariance = 0.05f }, Center);
                return true;
            }
            else //Fall through
            {
                velocity = new Vector2(p.velocity.X * 0.2f, 4 + p.velocity.Y * 0.5f) * 0.25f;

                if (p.velocity.Y < 1f)
                    p.velocity.Y = 0.5f;
                else
                    p.velocity.Y *= 0.5f;
                p.fallStart = (int)(position.Y / 16f);

                SpawnDust();

                SoundEngine.PlaySound(new SoundStyle("Verdant/Sounds/CloudLand") with { PitchVariance = 0.05f }, Center);

                BouncedUpon = true;
                bounceTimer = 40;
            }
            return false;
        }

        public override void Draw()
        {
            drawColor = Lighting.GetColor(position.ToTileCoordinates());
            drawPosition = Center;

            Main.spriteBatch.Draw(Texture.Value, drawPosition - Main.screenPosition, null, drawColor, rotation, source.Size() / 2f, 1f, SpriteEffects.None, 0);
        }
    }
}