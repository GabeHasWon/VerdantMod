using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Basic.Blocks
{
    internal class VerdantRedPetal : ModTile
    {
        public override void SetDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.SomethingRed, SoundID.Grass, new Color(216, 54, 43), ModContent.ItemType<RedPetal>(), "", true, false);
            QuickTile.MergeWith(Type, ModContent.TileType<LushSoil>(), ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<VerdantPinkPetal>(), TileID.LivingWood);
        }
    }
}