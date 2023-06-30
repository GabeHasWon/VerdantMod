using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.GameContent.Metadata;

namespace Verdant.Tiles.Verdant.Basic;

internal class HardmodeDecor1x1 : OmnidirectionalAnchorTile, IFlowerTile
{
    protected override int StyleRange => 10;

    protected override void StaticDefaults()
    {
        Main.tileCut[Type] = true;

        HitSound = SoundID.Grass;
        DustType = DustID.Grass;

        TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
        TileID.Sets.SwaysInWindBasic[Type] = true;
        AddMapEntry(new Color(161, 226, 99));
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(8) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}