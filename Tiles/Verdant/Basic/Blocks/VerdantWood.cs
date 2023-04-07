using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;

namespace Verdant.Tiles.Verdant.Basic.Blocks;

internal class VerdantWood : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.t_BorealWood, SoundID.Dig, new Color(142, 62, 32), ModContent.ItemType<VerdantWoodBlock>(), "", true, false);
        Main.tileBlendAll[Type] = true;
    }
}