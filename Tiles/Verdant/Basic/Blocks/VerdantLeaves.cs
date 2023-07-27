using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Blocks
{
    internal class VerdantLeaves : ModTile
    {
        public override void SetStaticDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.Grass, SoundID.Grass, new Color(71, 229, 32), string.Empty, true, false, true, false);
            QuickTile.MergeWith(Type, ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>());

            Main.tileCut[Type] = true;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            int reps = Main.rand.Next(2, 4);

            if (Main.netMode != NetmodeID.Server)
                for (int k = 0; k < reps; ++k)
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Vector2.Zero, Mod.Find<ModGore>("LushLeaf").Type);
        }
    }
}