using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Blocks
{
    internal class LushSoil : ModTile
    {
        public override void SetDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.Dirt, SoundID.Dig, new Color(91, 58, 28), ModContent.ItemType<LushSoilBlock>(), "", true, false);
            QuickTile.MergeWith(Type, TileID.Dirt, TileID.Mud, ModContent.TileType<VerdantSoilGrass>(), ModContent.TileType<VerdantPinkPetal>(), ModContent.TileType<VerdantRedPetal>());
        }
    }
}