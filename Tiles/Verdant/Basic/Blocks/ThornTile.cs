using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Blocks;

internal class ThornTile : ModTile
{
    public override bool IsLoadingEnabled(Mod mod) => false;

    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.Ash, SoundID.DD2_SkeletonHurt, new Color(68, 62, 50), "", true, false);
        QuickTile.MergeWith(Type, TileID.Dirt, TileID.Mud, ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<VerdantPinkPetal>(), ModContent.TileType<VerdantRedPetal>());
    }
}