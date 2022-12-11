using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Verdant.Systems.RealtimeGeneration.CaptureRendering
{
    internal class OverlayRenderer : ILoadable
    {
        private delegate void DrawSingleTileDelegate(TileDrawInfo drawData, bool solidLayer, int waterStyleOverride, Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY);
        private static DrawSingleTileDelegate DrawSingleTile;

        internal bool needsCapture = false;
        internal bool needsReset = false;
        internal CaptureData captureData = default;

        private RenderTarget2D tileTransitionOverlay = null;

        void ILoadable.Load(Mod mod)
        {
            DrawSingleTile = Delegate.CreateDelegate(typeof(DrawSingleTileDelegate), Main.instance.TilesRenderer, "DrawSingleTile") as DrawSingleTileDelegate;
        }

        void ILoadable.Unload() { }

        public static void Capture(Rectangle area, bool needsReset) => ModContent.GetInstance<OverlayRenderer>().CaptureInternal(area, needsReset);

        private void CaptureInternal(Rectangle area, bool needsReset)
        {
            this.needsReset = needsReset;
            needsCapture = true;
            captureData = new CaptureData(area);
        }

        public void Render()
        {
            if (needsReset)
            {
                tileTransitionOverlay = new(Main.graphics.GraphicsDevice, captureData.Area.Width, captureData.Area.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
                needsReset = false;
            }

            if (needsCapture)
            {
                RenderTarget();
                needsCapture = false;
            }

            if (tileTransitionOverlay is null)
                return;

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            Main.spriteBatch.Draw(tileTransitionOverlay, Main.MouseScreen, Color.White);
            Main.spriteBatch.End();
        }

        private void RenderTarget()
        {
            Main.graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

            var bindings = Main.graphics.GraphicsDevice.GetRenderTargets();
            Main.graphics.GraphicsDevice.SetRenderTarget(tileTransitionOverlay);
            Main.graphics.GraphicsDevice.Clear(Color.Black);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            if (!Main.gameMenu)
                DrawTiles();

            Main.spriteBatch.End();
            Main.graphics.GraphicsDevice.SetRenderTargets(bindings);

            Main.graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.DiscardContents;
        }

        private static void DrawTiles()
        {
            var offset = new Vector2(Main.offScreenRange, Main.offScreenRange);
            var pos = Main.MouseWorld.ToTileCoordinates();

            int x = pos.X;
            int y = pos.Y;

            for (int i = 0; i < 10; ++i)
                DrawSingleTile.Invoke(new TileDrawInfo(), true, -1, Main.Camera.UnscaledPosition, ModContent.GetInstance<OverlayRenderer>().captureData.Area.Location.ToVector2(), x + i, y + i);

            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, Main.MouseWorld, null, Color.White, 0f, Vector2.Zero, 20f, SpriteEffects.None, 0f);
        }
    }
}
