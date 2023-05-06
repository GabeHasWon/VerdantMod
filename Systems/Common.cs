using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            On.Terraria.Main.DrawNPCs += Main_DrawNPCs;
            On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
            On.Terraria.GameContent.Drawing.TileDrawing.Draw += TileDrawing_Draw;
            Main.OnTickForThirdPartySoftwareOnly += ScreenTextManager.Update;
        }

        private void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            orig(self);

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            DrawAdditiveProjectiles(AdditiveLayer.BeforePlayer);

            Main.spriteBatch.End();
        }

        private void Main_DrawNPCs(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behindTiles)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            DrawAdditiveNPCs(AdditiveLayer.BeforePlayer);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            orig(self, behindTiles);
        }

        private void TileDrawing_Draw(On.Terraria.GameContent.Drawing.TileDrawing.orig_Draw orig, TileDrawing self, bool solidLayer, bool forRenderTargets, bool intoRenderTargets, int waterStyleOverride)
        {
            orig(self, solidLayer, forRenderTargets, intoRenderTargets, waterStyleOverride);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null);

            Vector2 off = new(Main.offScreenRange);
            Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
            GetScreenDrawArea(unscaledPosition, off + (Main.Camera.UnscaledPosition - Main.Camera.ScaledPosition), out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);

            for (int i = firstTileX - 2; i < lastTileX + 2; i++)
            {
                for (int j = firstTileY; j < lastTileY + 4; j++)
                {
                    Tile tile = Main.tile[i, j];

                    if (tile.HasTile && tile.TileType >= TileID.Count && ModContent.GetModTile(tile.TileType) is IAdditiveTile add)
                        add.DrawAdditive(new Point16(i, j), AdditiveLayer.BeforePlayer);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        /// <summary>Copied from vanilla TileDrawing.</summary>
        private static void GetScreenDrawArea(Vector2 screenPosition, Vector2 offSet, out int firstTileX, out int lastTileX, out int firstTileY, out int lastTileY)
        {
            firstTileX = (int)((screenPosition.X - offSet.X) / 16f - 1f);
            lastTileX = (int)((screenPosition.X + Main.screenWidth + offSet.X) / 16f) + 2;
            firstTileY = (int)((screenPosition.Y - offSet.Y) / 16f - 1f);
            lastTileY = (int)((screenPosition.Y + Main.screenHeight + offSet.Y) / 16f) + 5;

            if (firstTileX < 4)
                firstTileX = 4;
            else if (lastTileX > Main.maxTilesX - 4)
                lastTileX = Main.maxTilesX - 4;

            if (firstTileY < 4)
                firstTileY = 4;
            else if (lastTileY > Main.maxTilesY - 4)
                lastTileY = Main.maxTilesY - 4;
        }

        private static void DrawForeground(On.Terraria.Main.orig_DrawGore orig, Main self)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            DrawAdditiveProjectiles(AdditiveLayer.AfterPlayer);
            DrawAdditiveNPCs(AdditiveLayer.AfterPlayer);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            orig(self);

            if (Main.PlayerLoaded && !Main.gameMenu)
                ForegroundManager.Draw();
        }

        private static void DrawAdditiveNPCs(AdditiveLayer layer)
        {
            foreach (var n in ActiveEntities.NPCs)
            {
                if (n.active && n.ModNPC is IDrawAdditive additive)
                    additive.DrawAdditive(layer);
            }
        }

        private static void DrawAdditiveProjectiles(AdditiveLayer layer)
        {
            foreach (var p in ActiveEntities.Projectiles)
            {
                if (p.active && p.ModProjectile is IDrawAdditive additive)
                    additive.DrawAdditive(layer);
            }
        }

        public void Unload() { }
    }
}
