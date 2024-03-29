﻿using Microsoft.Xna.Framework;
using Terraria;

namespace Verdant.Systems.Foreground.Parallax
{
    public class ApotheosisParticle : ParallaxedFGItem
    {
        private int lifeTimer = 0;
        private readonly int MaxLifeTimer = 0;

        public const int SpawnChance = 35;

        public ApotheosisParticle(Vector2 pos) : base(pos, Vector2.Zero, 1f, "Parallax/ApotheosisParticle")
        {
            parallax = Main.rand.Next(15, 150) * 0.01f;
            scale = parallax + 0.85f;
            MaxLifeTimer = Main.rand.Next(500, 800);

            source = new Rectangle(Main.rand.Next(3) * 5, 0, 4, 4);
            velocity = new Vector2(0, Main.rand.NextFloat(0.25f, 0.75f) * (parallax * 1.2f)).RotatedByRandom(MathHelper.Pi);
        }

        public override void Update()
        {
            base.Update();
            velocity = velocity.RotatedBy(0.01f * parallax);

            lifeTimer++;

            if (lifeTimer > MaxLifeTimer)
                killMe = true;
        }

        public override void Draw()
        {
            float alphMult = 1f;// (1 - (parallax - 0.15f) / 1.35f) * 0.5f + 0.5f;
            float alpha = 1f;

            if (lifeTimer < 100)
                alpha = lifeTimer / 100f;
            if (lifeTimer > MaxLifeTimer - 100)
                alpha = (MaxLifeTimer - lifeTimer) / 100f;

            drawColor = Color.White * (alpha * alphMult);
            drawPosition = position + ParallaxPosition();

            if (lifeTimer % 12 == 0)
            {
                source.Y += 5;
                if (source.Y >= 15)
                    source.Y = 0;
            }

            base.Draw();
        }
    }
}