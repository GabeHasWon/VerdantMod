using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Puff;

class BigPuff : ModTile, IFlowerTile
{
    public const int FrameHeight = 38;
    public const int MaxFrame = 4;

    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>(), TileID.HallowedGrass, TileID.Grass, TileID.JungleGrass, ModContent.TileType<VerdantPinkPetal>() };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassList());

        QuickTile.SetMulti(this, 2, 3, DustID.PinkStarfish, SoundID.Grass, false, new Color(255, 112, 202), true, false, origin: new Point16(0, 1));
    }

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(16) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}