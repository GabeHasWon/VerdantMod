using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Verdant.Systems.ScreenText.Animations;

namespace Verdant.Systems.ScreenText
{
    internal class ScreenText
    {
        public enum Alignment
        {
            Left,
            Center,
            Right,
        }

        public enum DrawEffect
        {
            None,
            Subtle,
        }

        private readonly float MaxTimeLeft = 0;
        private readonly bool AutomaticallyDie = true;

        public ScreenText Next { get; private set; } = null;

        public string text = string.Empty;
        public float scale = 1f;
        public Alignment alignment = Alignment.Center;
        public DrawEffect effect = DrawEffect.None;
        public IScreenTextAnimation anim = new DefaultAnimation();
        public int timeLeft = 0;
        public bool final = false;

        internal bool active = true;

        private Action<ScreenText> _onFinish = null;

        public ScreenText(string text, int timeLeft, float scale = 1f, Alignment alignment = Alignment.Center, DrawEffect effect = DrawEffect.None, IScreenTextAnimation anim = null, bool dieAutomatically = true)
        {
            this.text = text;
            this.scale = scale;
            this.alignment = alignment;
            this.effect = effect;
            this.timeLeft = timeLeft;
            this.anim = anim ?? new DefaultAnimation();

            MaxTimeLeft = timeLeft;
            AutomaticallyDie = dieAutomatically;
        }

        public void Update()
        {
            timeLeft--;

            if (AutomaticallyDie && timeLeft <= 0 && Main.mouseRight)
            {
                active = false;
                _onFinish?.Invoke(this);
            }
        }

        public void Draw()
        {
            const string RightClick = "Right click to continue";

            float realFactor = timeLeft / MaxTimeLeft;
            float factor = MathHelper.Clamp(realFactor, 0, 1);
            int textSize = (int)(text.Length * (1f - factor));
            string showText = text[..textSize];

            var font = FontAssets.DeathText;
            Vector2 size = font.Value.MeasureString(showText);
            float xOffset = 0;

            switch (alignment)
            {
                case Alignment.Left:
                    xOffset = -size.X / 2f;
                    break;
                case Alignment.Right:
                    xOffset = size.X / 2f;
                    break;
                case Alignment.Center:
                default:
                    break;
            }

            Vector2 pos = new(Main.screenWidth / 2f + xOffset, Main.screenHeight * 0.1f);
            float drawScale = scale;
            Color col = Color.White;

            anim.ModifyDraw(realFactor, this, ref pos, ref col, ref drawScale);

            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font.Value, showText, pos, col, 0f, Vector2.UnitX * size.X / 2f, Vector2.One * drawScale);

            if (timeLeft <= 0 && AutomaticallyDie)
            {
                Vector2 rSiz = font.Value.MeasureString(RightClick);
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font.Value, RightClick, pos + (Vector2.UnitY * rSiz.Y), Color.Gray * 0.5f, 0f, Vector2.UnitX * rSiz.X / 2f, Vector2.One * 0.5f);
            }
        }

        public void DrawAdditive()
        {

        }

        public ScreenText With(ScreenText other)
        {
            SetNext(other);
            return this;
        }

        public ScreenText FinishWith(ScreenText other, Action<ScreenText> action = null)
        {
            other._onFinish = action;
            other.final = true;
            SetNext(other);
            return this;
        }

        protected void SetNext(ScreenText other)
        {
            if (Next is null)
                Next = other;
            else
                Next.SetNext(other);
        }
    }
}
