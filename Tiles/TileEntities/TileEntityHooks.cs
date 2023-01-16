using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

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

            orig(self, behind);
        }
    }
}
