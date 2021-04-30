using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace Verdant.Backgrounds.BGItem.Verdant
{
    public class LushLeafBG : BaseBGItem
    {
        private int timer = 0;
        private int offscreenTimer = 0;

        public const int SpawnChance = 35;

        public LushLeafBG(Vector2 pos) : base(pos, 1f, new Point(26, 22))
        {
            tex = Terraria.ModLoader.ModContent.GetTexture("Verdant/Backgrounds/BGItem/Verdant/LushLeaf");

            int r = Main.rand.Next(30);
            if (r < 4)
                source.Y = 23;
            else if (r > 25)
                source.Y = 46;
            else
                source.Y = Main.rand.NextBool() ? 0 : 69;

            parallaxScale = Main.rand.Next(640, 801) * 0.001f;
            scale = Main.rand.NextFloat(0.70f, 1.15f);
            timer = Main.rand.Next(10000);
        }

        internal override void Behaviour()
        {
            base.Behaviour();
            float xVel = (float)Math.Sin(timer++ * 0.036) * 0.6f * scale;
            velocity.X = xVel + Main.windSpeed;
            velocity.Y = -Math.Abs(xVel) + scale;

            if (!new Rectangle((int)Main.screenPosition.X - 60, (int)Main.screenPosition.Y - 60, Main.screenWidth + 120, Main.screenHeight + 120).Contains(Center.ToPoint()))
                offscreenTimer++;
            else
                offscreenTimer = 0;

            if (offscreenTimer > 900)
                killMe = true;

            BaseParallax(0.55f);
        }

        internal override void Draw(Vector2 off)
        {
            if (velocity.X < -0.8f * parallaxScale)
                source.Location = new Point(0, source.Location.Y);
            else if (velocity.X < -0.4f * parallaxScale)
                source.Location = new Point(27, source.Location.Y);
            else if (velocity.X < 0.4f * parallaxScale)
                source.Location = new Point(54, source.Location.Y);
            else if (velocity.X < 0.8f * parallaxScale)
                source.Location = new Point(81, source.Location.Y);
            else
                source.Location = new Point(108, source.Location.Y);
            drawColor = Main.bgColor;
            base.Draw(GetParallax());
        }
    }
}
