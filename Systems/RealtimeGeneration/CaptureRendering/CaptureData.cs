using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace Verdant.Systems.RealtimeGeneration.CaptureRendering
{
    public abstract class CaptureData
    {
        internal readonly Effect Effect;
        internal readonly string Name;
        internal readonly bool ContinueDrawing = true;

        public Rectangle Area { get; internal set; }

        public CaptureData(string name)
        {
            Effect = null;
            Name = name;
        }

        public CaptureData(Effect effect)
        {
            Effect = effect;
        }

        internal virtual void ApplyRenderSpriteBatch() => Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null);
        internal virtual void ApplyNormalSpriteBatch() => Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, Effect, Main.UIScaleMatrix);
        internal virtual void DrawTarget(RenderTarget2D overlay) => Main.spriteBatch.Draw(overlay, Main.MouseScreen - overlay.Size() / 2, Color.White);

        internal virtual void Update() { }
    }

    internal class DefaultCapture : CaptureData
    {
        public DefaultCapture(string name) : base(name) { }
    }
}
