﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Plants;

internal class RootRopeTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = false;
        Main.tileCut[Type] = false;
        Main.tileMergeDirt[Type] = false;
        Main.tileBlockLight[Type] = false;
        Main.tileRope[Type] = true;
        Main.tileFrameImportant[Type] = false;

        AddMapEntry(new Color(165, 108, 58));

        DustType = DustID.Dirt;
        HitSound = SoundID.Dig;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;
}