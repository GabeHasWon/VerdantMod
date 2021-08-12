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
    internal class VerdantRedPetal : ModTile
    {
        public override void SetDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.SomethingRed, SoundID.Grass, new Color(216, 54, 43), ItemType<RedPetal>(), "", true, false);
            QuickTile.MergeWith(Type, TileType<LushSoil>(), TileType<VerdantGrassLeaves>(), TileType<VerdantPinkPetal>(), TileID.LivingWood);
        }
    }
}