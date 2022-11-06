using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.UI.Chat;

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

        public enum Effect
        {
            None,
            Subtle,
        }

        public ScreenText Next { get; private set; } = null;

        public string text = string.Empty;
        public float scale = 1f;
        public Alignment alignment = Alignment.Center;
        public Effect effect = Effect.None;
        public int timeLeft = 0;
        public bool final = false;

        internal bool active = true;

        private Action<ScreenText> _onFinish = null;

        public ScreenText(string text, int timeLeft, float scale = 1f, Alignment alignment = Alignment.Center, Effect effect = Effect.None)
        {
            this.text = text;
            this.scale = scale;
            this.alignment = alignment;
            this.effect = effect;
            this.timeLeft = timeLeft;
        }

        public void Update()
        {
            timeLeft--;

            if (timeLeft <= 0 && Main.mouseRight)
            {
                active = false;
                _onFinish?.Invoke(this);
            }
        }

        public void Draw()
        {
            const string RightClick = "Right click to continue";

            var font = FontAssets.DeathText;
            Vector2 size = font.Value.MeasureString(text);
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
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font.Value, text, pos, Color.White, 0f, Vector2.UnitX * size.X / 2f, Vector2.One * scale * 1);

            if (timeLeft <= 0)
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
