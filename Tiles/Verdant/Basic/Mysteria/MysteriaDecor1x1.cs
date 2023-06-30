using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Metadata;

namespace Verdant.Tiles.Verdant.Basic.Mysteria;

internal class MysteriaDecor1x1 : OmnidirectionalAnchorTile, IFlowerTile
{
    protected override int StyleRange => 10;

    protected override void StaticDefaults()
    {
        HitSound = SoundID.Grass;
        DustType = DustID.Grass;

        TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
        TileID.Sets.SwaysInWindBasic[Type] = true;
        Main.tileCut[Type] = true;

        AddMapEntry(new Color(148, 113, 207));
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;
    public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) => TileHelper.CrystalSetSpriteEffects(i, j, ref spriteEffects);

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(8) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}