using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Aquamarine;

internal class BackslateTile : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.UnusedWhiteBluePurple, SoundID.Dig, new Color(183, 201, 194));

        Main.tileBrick[Type] = true;
    }
}