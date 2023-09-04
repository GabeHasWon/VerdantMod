using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Blocks;

internal class LushSoil : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.Dirt, SoundID.Dig, new Color(91, 58, 28), true, false);
        QuickTile.MergeWith(Type, TileID.Dirt, TileID.Mud, ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<VerdantPinkPetal>(), 
            ModContent.TileType<VerdantRedPetal>(), TileID.Glass, TileID.Chlorophyte);

        Main.tileBrick[Type] = true;
    }
}