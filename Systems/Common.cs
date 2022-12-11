using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Drawing;
using Verdant.Systems.Foreground;
using Verdant.Systems.ScreenText;

namespace Verdant.Systems
{
    internal class Common : ILoadable
    {
        public void Load(Mod mod)
        {
            On.Terraria.Main.DrawGore += DrawForeground;
            On.Terraria.Main.DrawCursor += Main_DrawCursor;
            On.Terraria.GameContent.Drawing.TileDrawing.Draw += TileDrawing_Draw;
            Main.OnTickForThirdPartySoftwareOnly += ScreenTextManager.Update;
        }

        private void TileDrawing_Draw(On.Terraria.GameContent.Drawing.TileDrawing.orig_Draw orig, TileDrawing self, bool solidLayer, bool forRenderTargets, bool intoRenderTargets, int waterStyleOverride)
        {
            orig(self, solidLayer, forRenderTargets, intoRenderTargets, waterStyleOverride);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Vector2 off = new(Main.offScreenRange);
            Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
            GetScreenDrawArea(unscaledPosition, off + (Main.Camera.UnscaledPosition - Main.Camera.ScaledPosition), out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);

            for (int i = firstTileX - 2; i < lastTileX + 2; i++)
            {
                for (int j = firstTileY; j < lastTileY + 4; j++)
                {
                    Tile tile = Main.tile[i, j];

                    if (tile.HasTile && tile.TileType >= TileID.Count)
                        if (ModContent.GetModTile(tile.TileType) is IAdditiveTile add)
                            add.DrawAdditive(new Point16(i, j));
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        /// <summary>Copied from vanilla TileDrawing.</summary>
        private void GetScreenDrawArea(Vector2 screenPosition, Vector2 offSet, out int firstTileX, out int lastTileX, out int firstTileY, out int lastTileY)
        {
            firstTileX = (int)((screenPosition.X - offSet.X) / 16f - 1f);
            lastTileX = (int)((screenPosition.X + (float)Main.screenWidth + offSet.X) / 16f) + 2;
            firstTileY = (int)((screenPosition.Y - offSet.Y) / 16f - 1f);
            lastTileY = (int)((screenPosition.Y + (float)Main.screenHeight + offSet.Y) / 16f) + 5;

            if (firstTileX < 4)
                firstTileX = 4;
            else if (lastTileX > Main.maxTilesX - 4)
                lastTileX = Main.maxTilesX - 4;

            if (firstTileY < 4)
                firstTileY = 4;
            else if (lastTileY > Main.maxTilesY - 4)
                lastTileY = Main.maxTilesY - 4;
        }

        private void Main_DrawCursor(On.Terraria.Main.orig_DrawCursor orig, Microsoft.Xna.Framework.Vector2 bonus, bool smart)
        {
            if (!Main.gameMenu)
                ScreenTextManager.Render();

            orig(bonus, smart);
        }

        private static void DrawForeground(On.Terraria.Main.orig_DrawGore orig, Main self)
        {
            orig(self);

            if (Main.PlayerLoaded && !Main.gameMenu)
                ForegroundManager.Draw();

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            DrawAdditiveProjectiles();

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        private static void DrawAdditiveProjectiles()
        {
            for (int i = 0; i < Main.maxProjectiles; ++i)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.ModProjectile is IDrawAdditive additive)
                    additive.DrawAdditive(AdditiveLayer.AfterPlayer);
            }
        }

        public void Unload() { }
    }
}
