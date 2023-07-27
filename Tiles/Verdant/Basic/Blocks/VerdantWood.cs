using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Blocks;

internal class VerdantWood : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.t_BorealWood, SoundID.Dig, new Color(142, 62, 32), "", true, false);
        Main.tileBrick[Type] = true;
    }
}