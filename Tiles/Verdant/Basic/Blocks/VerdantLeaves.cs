using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Blocks
{
    internal class VerdantLeaves : ModTile
    {
        public override void SetDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.Grass, SoundID.Grass, new Color(71, 229, 32), -1, "", true, false, true, false);
            QuickTile.MergeWith(Type, ModContent.TileType<VerdantSoilGrass>(), ModContent.TileType<LushSoil>());

            Terraria.Main.tileCut[Type] = true;
        }
    }
}