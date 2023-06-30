using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.GameContent.Metadata;
using Terraria;

namespace Verdant.Tiles.Verdant.Basic.Cut;

internal class MossDecor1x1 : OmnidirectionalAnchorTile, IFlowerTile
{
    protected override int StyleRange => 4;

    protected override void StaticDefaults()
    {
        Main.tileCut[Type] = true;

        TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
        TileID.Sets.SwaysInWindBasic[Type] = true;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(8) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}