using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
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
            Main.OnTickForThirdPartySoftwareOnly += ScreenTextManager.Update;
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
                if (p.active && p.ModProjectile is Drawing.IDrawAdditive additive)
                    additive.DrawAdditive(Drawing.AdditiveLayer.AfterPlayer);
            }
        }

        public void Unload() { }
    }
}
