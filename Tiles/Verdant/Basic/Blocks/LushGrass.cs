using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Blocks;

internal class LushGrass : ModTile, IVerdantGrassTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.Grass, SoundID.Dig, new Color(84, 178, 38), true, false);

        Main.tileBrick[Type] = true;

        TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<LushSoil>();
        TileID.Sets.NeedsGrassFraming[Type] = true;
    }

    public override bool CanExplode(int i, int j)
    {
        WorldGen.KillTile(i, j, false, false, false);
        return true;
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (!fail)
        {
            fail = true;
            Main.tile[i, j].TileType = (ushort)ModContent.TileType<LushSoil>();
        }
    }

    public override void RandomUpdate(int i, int j)
    {
        VerdantGrassLeaves.StaticRandomUpdate(i, j);
        TileHelper.Spread(i, j, Type, 4, true, ModContent.TileType<LushSoil>());
    }
}