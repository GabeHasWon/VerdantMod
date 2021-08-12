using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Tiles.Verdant.Basic.Blocks
{
    internal class VerdantPinkPetal : ModTile
    {
        public override void SetDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.SomethingRed, SoundID.Grass, new Color(228, 155, 174), ItemType<PinkPetal>(), "", true, false);
            QuickTile.MergeWith(Type, TileType<LushSoil>(), TileType<VerdantGrassLeaves>(), TileType<VerdantRedPetal>(), TileID.LivingWood);
        }
    }
}