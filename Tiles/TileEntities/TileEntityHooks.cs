using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Verdant.Tiles.TileEntities
{
    internal class TileEntityHooks : ModSystem
    {
        public override void Load()
        {
            On.Terraria.Main.DrawNPCs += Main_DrawProjectiles;
        }

        private void Main_DrawProjectiles(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behind)
        {
            orig(self, behind);

            foreach (var item in TileEntity.ByID)
            {
                if (item.Value is DrawableTE te && te.CanDraw())
                    te.Draw(Main.spriteBatch);
            }
        }
    }
}
