using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Gores.Verdant;

namespace Verdant.Tiles.Verdant.Basic.Blocks
{
    internal class VerdantLeaves : ModTile
    {
        public override void SetDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.Grass, SoundID.Grass, new Color(71, 229, 32), -1, "", true, false, true, false);
            QuickTile.MergeWith(Type, ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>());

            Main.tileCut[Type] = true;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            int reps = Main.rand.Next(2, 4);
            for (int k = 0; k < reps; ++k)
                Gore.NewGore(new Vector2(i, j) * 16, Vector2.Zero, mod.GetGoreSlot("Gores/Verdant/LushLeaf"));
        }
    }
}