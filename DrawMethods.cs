using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Verdant.Backgrounds.BGItem;

namespace Verdant
{
    public partial class VerdantMod : Mod
    {
        private void Main_DrawBackgroundBlackFill(On.Terraria.Main.orig_DrawBackgroundBlackFill orig, Main self)
        {
            orig(self);

            if (Main.playerLoaded && BackgroundItemManager.Loaded && !Main.gameMenu)
                BackgroundItemManager.Draw();
        }

        private void Main_DrawGore(On.Terraria.Main.orig_DrawGore orig, Main self)
        {
            orig(self);

            if (Main.playerLoaded && !Main.gameMenu)
                Foreground.ForegroundManager.Run();

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < Main.maxProjectiles; ++i)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.modProjectile is Drawing.IDrawAdditive additive)
                    additive.DrawAdditive(Drawing.AdditiveLayer.AfterPlayer);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}