﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Verdant.Foreground.Parallax
{
    public class CloudbloomEntity : ParallaxedFGItem
    {
        const int Width = 58;

        private bool BouncedUpon = false;
        private int bounceTimer = 0;
        private int rotTimer = 0;
        internal Vector2 anchor = Vector2.Zero;

        public CloudbloomEntity(Vector2 pos) : base(pos - new Vector2(58, 38) / 2f, Vector2.Zero, 1f, "Parallax/CloudbloomEntity")
        {
            source = new Rectangle(0, 0, 58, 38);
            velocity = new Vector2(0, Main.rand.NextFloat(0.25f, 0.75f) * (parallax * 1.2f)).RotatedByRandom(MathHelper.Pi);
            anchor = Center;
            parallax = 1f;
        }

        public override void Update()
        {
            base.Update();

            velocity *= 0.9f;
            bounceTimer++;
            rotation = (float)Math.Sin(rotTimer++ * 0.05f) * 0.28f;

            if (Main.rand.NextBool(14))
                Dust.NewDustPerfect(new Vector2(position.X + 4 + Main.rand.Next(Width - 8), Center.Y), DustID.Cloud, new Vector2(0, Main.rand.NextFloat(0.2f, 0.5f)));

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
                velocity += (Center == anchor) ? Vector2.Zero : Vector2.Normalize(anchor - Center) * 0.5f;
                scale = (bounceTimer / 60f) * 0.25f + 0.75f;

                if (Vector2.DistanceSquared(Center, anchor) < 2 * 2)
                    velocity *= 0.98f;

                if (bounceTimer > 60)
                    BouncedUpon = false;
            }
        }

        private bool BounceOnPlayer(Player p)
        {
            if (!p.controlDown) //Jump up
            {
                velocity = new Vector2(p.velocity.X * 0.2f, 4 + p.velocity.Y * 0.5f);
                p.velocity.Y = -14;
                p.velocity.X *= 1.2f;
                p.fallStart = (int)(position.Y / 16f);

                BouncedUpon = true;
                bounceTimer = 0;

                for (int j = 0; j < 14; ++j)
                    Dust.NewDust(position + new Vector2(4, 12), Width - 8, 8, DustID.Cloud, Main.rand.NextFloat(-0.2f, 0.2f) + velocity.X, Main.rand.NextFloat(2, 3f));

                SoundEngine.PlaySound(new SoundStyle("Verdant/Sounds/CloudJump") with { PitchVariance = 0.05f }, Center);
                return true;
            }
            else //Fall through
            {
                velocity = new Vector2(p.velocity.X * 0.2f, 4 + p.velocity.Y * 0.5f) * 0.25f;
                p.velocity.Y *= 0.5f;
                p.fallStart = (int)(position.Y / 16f);

                for (int j = 0; j < 14; ++j)
                    Dust.NewDust(position + new Vector2(4, 12), Width - 8, 8, DustID.Cloud, Main.rand.NextFloat(-0.2f, 0.2f) + velocity.X, Main.rand.NextFloat(2, 3f));

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

            Main.spriteBatch.Draw(VerdantMod.Instance.Assets.Request<Texture2D>("Foreground/Parallax/CloudbloomEntity").Value, drawPosition - Main.screenPosition, null, drawColor, rotation, source.Size() / 2f, 1f, SpriteEffects.None, 0);
            //base.Draw();
        }
    }
}