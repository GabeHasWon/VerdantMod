using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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
        public string speaker = string.Empty;
        public float scale = 1f;
        public Alignment alignment = Alignment.Center;
        public DrawEffect effect = DrawEffect.None;
        public IScreenTextAnimation anim = new DefaultAnimation();
        public float timeLeft = 0;
        public bool final = false;
        public Color color = Color.White;
        public Color speakerColor = Color.White;
        public Asset<Effect> shader = null;
        public ScreenTextEffectParameters shaderParams = new ScreenTextEffectParameters();

        internal bool active = true;

        private Action<ScreenText> _onFinish = null;

        /// <summary>Create a new ScreenText with the given parameters.</summary>
        /// <param name="text">If <paramref name="useTranslationKey"/> is false, the text to use. If not, the key to use to get the text.</param>
        /// <param name="timeLeft">How long the text takes to tick.</param>
        /// <param name="scale">Size of the text.</param>
        /// <param name="alignment">Right, left or center alignment. Center is the only implemented function at this moment.</param>
        /// <param name="effect">I don't remember lol</param>
        /// <param name="anim">Animation (such as fadeout) to use.</param>
        /// <param name="dieAutomatically">Whether this ScreenText stops showing up by itself or not.</param>
        public ScreenText(string text, float timeLeft, float scale = 1f, Alignment alignment = Alignment.Center, DrawEffect effect = DrawEffect.None, IScreenTextAnimation anim = null, bool dieAutomatically = true)
        {
            this.text = VerdantLocalization.ScreenTextLocalization(text);
            this.scale = scale;
            this.alignment = alignment;
            this.effect = effect;
            this.timeLeft = timeLeft;
            this.anim = anim ?? new DefaultAnimation();

            MaxTimeLeft = timeLeft;
            AutomaticallyDie = dieAutomatically;
        }

        public void Update(GameTime gameTime)
        {
            timeLeft -= gameTime.ElapsedGameTime.Milliseconds / 16f;

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
            float xOffset = 0;
            float drawScale = scale;
            Color speakerCol = speakerColor;

            Vector2 size = font.Value.MeasureString(showText);

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
            Color col = color;

            anim.ModifyDraw(realFactor, this, ref pos, ref col, ref speakerCol, ref drawScale);

            Vector2 speakerSize = font.Value.MeasureString(speaker);
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font.Value, speaker, pos - (Vector2.UnitY * speakerSize.Y * 0.7f), speakerCol, 0f, Vector2.UnitX * speakerSize.X / 2f, Vector2.One * 0.6f);
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font.Value, showText, pos, col, 0f, Vector2.UnitX * size.X / 2f, Vector2.One * drawScale); //Actual draw text

            if (timeLeft <= 0 && AutomaticallyDie)
            {
                Vector2 rSiz = font.Value.MeasureString(RightClick);
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font.Value, RightClick, pos + (Vector2.UnitY * rSiz.Y), Color.Gray * 0.5f, 0f, Vector2.UnitX * rSiz.X / 2f, Vector2.One * 0.4f);
            }
        }

        public void DrawAdditive()
        {

        }

        public ScreenText With(ScreenText other, bool sameSpeaker = true)
        {
            SetNext(other, sameSpeaker);
            return this;
        }

        public ScreenText FinishWith(ScreenText other, Action<ScreenText> action = null, bool sameSpeaker = true)
        {
            other._onFinish = action;
            other.final = true;
            SetNext(other, sameSpeaker);
            return this;
        }

        protected void SetNext(ScreenText other, bool sameSpeaker = true)
        {
            if (Next is null)
            {
                if (sameSpeaker)
                {
                    other.speaker = speaker;
                    other.speakerColor = speakerColor;
                }

                other.shader = shader;
                other.shaderParams = shaderParams;
                other.color = color;
                Next = other;
            }
            else
                Next.SetNext(other, sameSpeaker);
        }
    }
}
