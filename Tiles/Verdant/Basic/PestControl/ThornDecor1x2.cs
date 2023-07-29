using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.PestControl;

internal class ThornDecor1x2 : ModTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<ThornTile>() };
        TileObjectData.newTile.RandomStyleRange = 6;
        TileObjectData.newTile.StyleHorizontal = true;
        QuickTile.SetMulti(this, 1, 2, DustID.Grass, SoundID.Grass, true, new Color(112, 112, 112));
    }

    public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
}
