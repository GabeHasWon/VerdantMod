using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Verdant.Drawing;

namespace Verdant.Tiles.TileEntities
{
    internal class TileEntityHooks : ILoadable
    {
        public void Load(Mod mod)
        {
            On.Terraria.Main.DrawNPCs += DrawTEs;
        }

        void ILoadable.Unload() { }

        private void DrawTEs(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behind)
        {
            if (behind)
            {
                orig(self, behind);
                return;
            }

            foreach (var item in TileEntity.ByID)
            {
                if (item.Value is DrawableTE te && te.CanDraw())
                    te.Draw(Main.spriteBatch);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (var item in TileEntity.ByID)
            {
                if (item.Value is DrawableTE te && te.CanDraw() && te is IDrawAdditive additive)
                    additive.DrawAdditive(AdditiveLayer.BeforePlayer);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            orig(self, behind);
        }
    }
}
