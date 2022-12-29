using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Plants;

class YellowSprouts : ModTile, IFlowerTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>() };
        TileObjectData.newTile.RandomStyleRange = 3;
        TileObjectData.newTile.StyleHorizontal = true;

        QuickTile.SetMulti(this, 2, 2, DustID.YellowStarfish, SoundID.Grass, true, new Color(224, 120, 0));
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, new Vector2(32, 32), ModContent.ItemType<Items.Verdant.Materials.YellowBulb>(), 1);

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(16) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}