using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Blocks;

public class MysteriaFluff : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.PurpleMoss, SoundID.Dig, new Color(113, 86, 158), true, false);
        Main.tileBrick[Type] = true;
    }
}