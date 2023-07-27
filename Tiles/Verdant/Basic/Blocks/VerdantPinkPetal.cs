using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Basic.Blocks
{
    internal class VerdantPinkPetal : ModTile
    {
        public override void SetStaticDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.SomethingRed, SoundID.Grass, new Color(228, 155, 174), "", true, false);
            QuickTile.MergeWith(Type, ModContent.TileType<LushSoil>(), ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<VerdantRedPetal>(), TileID.LivingWood);

            RegisterItemDrop(ModContent.ItemType<PinkPetal>());
        }
    }
}