using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Verdant.Tiles.Verdant.Basic.PestControl;

internal class ThornDecor1x1 : ModTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<ThornTile>() };
        TileObjectData.newTile.RandomStyleRange = 4;
        TileObjectData.newTile.StyleHorizontal = true;
        QuickTile.SetMulti(this, 1, 1, DustID.Stone, SoundID.Dig, false, new Color(112, 112, 112));
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;
    public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
}