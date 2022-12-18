using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        internal CaptureData currentCaptureData = default;

        internal List<CaptureData> capturedOverlays = new();

        private RenderTarget2D tileTransitionOverlay = null;

        void ILoadable.Load(Mod mod) => DrawSingleTile = Delegate.CreateDelegate(typeof(DrawSingleTileDelegate), Main.instance.TilesRenderer, "DrawSingleTile") as DrawSingleTileDelegate;
        void ILoadable.Unload() => DrawSingleTile = null;

        public static void Capture(bool needsReset, CaptureData data) => ModContent.GetInstance<OverlayRenderer>().CaptureInternal(needsReset, data);

        private void CaptureInternal(bool needsReset, CaptureData data)
        {
            this.needsReset = needsReset;
            needsCapture = true;
            currentCaptureData = data;
        }

        public void Render()
        {
            if (needsReset)
            {
                tileTransitionOverlay = new(Main.graphics.GraphicsDevice, currentCaptureData.Area.Width, currentCaptureData.Area.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
                needsReset = false;
            }

            if (needsCapture)
            {
                RenderTarget();
                needsCapture = false;

                currentCaptureData.Capture = tileTransitionOverlay;
                capturedOverlays.Add(currentCaptureData);
            }

            if (tileTransitionOverlay is null)
                return;

            foreach (var item in capturedOverlays)
            {
                if (item.Run)
                    item.Update();

                if (!item.Draw)
                    continue;

                item.ApplyNormalSpriteBatch();
                item.DrawTarget(item.Capture);
                Main.spriteBatch.End();
            }
        }

        private void RenderTarget()
        {
            Main.graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

            var bindings = Main.graphics.GraphicsDevice.GetRenderTargets();
            Main.graphics.GraphicsDevice.SetRenderTarget(tileTransitionOverlay);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);
            currentCaptureData.ApplyRenderSpriteBatch();

            if (!Main.gameMenu)
                DrawTiles();

            Main.spriteBatch.End();
            Main.graphics.GraphicsDevice.SetRenderTargets(bindings);

            Main.graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.DiscardContents;
        }

        private static void DrawTiles()
        {
            var area = ModContent.GetInstance<OverlayRenderer>().currentCaptureData.Area;
            var offset = new Vector2(Main.offScreenRange, Main.offScreenRange);
            var pos = (Main.MouseWorld - (area.Size() / 2)).ToTileCoordinates();

            int x = pos.X;
            int y = pos.Y;

            for (int i = 0; i < area.Width / 16; ++i)
            {
                for (int j = 0; j < area.Height / 16; ++j)
                {
                    if (!Main.tile[x + i, y + j].HasTile)
                        continue;

                    if (Lighting.Brightness(x + i, y + j) == 0f)
                        Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Vector2(i, j) * 16, new Rectangle(0, 0, 16, 16), Color.Black);

                    var off = -area.Location.ToVector2() + Main.screenPosition;
                    DrawSingleTile.Invoke(new TileDrawInfo(), true, -1, Main.Camera.UnscaledPosition, off, x + i, y + j);
                }
            }
        }
    }
}
