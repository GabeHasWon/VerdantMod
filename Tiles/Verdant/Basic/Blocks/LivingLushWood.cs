using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Blocks
{
    internal class LivingLushWood : ModTile
    {
        public override void SetDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.Dirt, SoundID.Dig, new Color(89, 47, 33), ModContent.ItemType<Items.Verdant.Blocks.LushWood.VerdantWoodBlock>(), "", true, false);
            QuickTile.MergeWith(Type, TileID.Dirt, TileID.Mud, ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<VerdantPinkPetal>(), ModContent.TileType<VerdantRedPetal>());

            Main.tileBrick[Type] = true;
        }
    }
}