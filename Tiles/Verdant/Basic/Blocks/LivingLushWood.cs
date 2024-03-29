﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;

namespace Verdant.Tiles.Verdant.Basic.Blocks
{
    internal class LivingLushWood : ModTile
    {
        public override void SetStaticDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.Dirt, SoundID.Dig, new Color(89, 47, 33), true, false);
            QuickTile.MergeWith(Type, TileID.Dirt, TileID.Mud, ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<VerdantPinkPetal>(), 
                ModContent.TileType<VerdantRedPetal>(), ModContent.TileType<LushSoil>());

            Main.tileBrick[Type] = true;

            RegisterItemDrop(ModContent.ItemType<VerdantWoodBlock>());
        }
    }
}