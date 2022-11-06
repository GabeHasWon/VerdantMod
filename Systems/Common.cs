using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Verdant.Systems.Foreground;

namespace Verdant.Systems
{
    internal class Common : ILoadable
    {
        public void Load(Mod mod)
        {
            On.Terraria.Main.DrawGore += DrawForeground;
            Main.OnTickForThirdPartySoftwareOnly += ScreenText.ScreenTextManager.Update;
        }

        private static void DrawForeground(On.Terraria.Main.orig_DrawGore orig, Main self)
        {
            orig(self);

            if (Main.PlayerLoaded && !Main.gameMenu)
                ForegroundManager.Draw();

            ScreenText.ScreenTextManager.Draw();

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            DrawAdditiveProjectiles();
            ScreenText.ScreenTextManager.DrawAdditive();

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
