using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Marble;

namespace Verdant.Tiles.Verdant.Decor.Marble;

class ThePlant : ModTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
        TileObjectData.newTile.StyleHorizontal = true;
        QuickTile.SetMulti(this, 2, 2, DustID.MarblePot, SoundID.Shatter, true, new Color(161, 226, 99));
        RegisterItemDrop(ModContent.ItemType<ThePlantItem>());
    }
}
