using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Metadata;
using System.Linq;

namespace Verdant.Tiles.Verdant.Basic;

internal class VerdantDecor1x1 : ModTile, IFlowerTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>() };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassTypes.ToList());
        TileObjectData.newTile.RandomStyleRange = 7;
        TileObjectData.newTile.StyleHorizontal = true;
        QuickTile.SetMulti(this, 1, 1, DustID.Grass, SoundID.Grass, false, new Color(161, 226, 99));
        Main.tileCut[Type] = true;

        TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
        TileID.Sets.SwaysInWindBasic[Type] = true;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;
    public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(12, 6), new Vector2(8, 10) };
    public bool IsFlower(int i, int j) => Main.tile[i, j].TileFrameX != 18 && Main.tile[i, j].TileFrameX != 36;

    public Vector2[] OffsetAt(int i, int j)
    {
        var offsets = GetOffsets();
        Tile tile = Main.tile[i, j];

        if (tile.TileFrameX == 0)
            return new[] { offsets[0] };
        return new[] { offsets[1] };
    }
}

internal class Decor1x1Right : ModTile, IFlowerTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
        TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 0);
        TileObjectData.newTile.RandomStyleRange = 7;
        TileObjectData.newTile.StyleHorizontal = true;
        QuickTile.SetMulti(this, 1, 1, DustID.Grass, SoundID.Grass, false, new Color(161, 226, 99));

        Main.tileCut[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileNoFail[Type] = true;

        TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
        TileID.Sets.SwaysInWindBasic[Type] = true;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;
    public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (j % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipVertically;

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(12, 6), new Vector2(8, 10) };
    public bool IsFlower(int i, int j) => Main.tile[i, j].TileFrameX != 18 && Main.tile[i, j].TileFrameX != 36;

    public Vector2[] OffsetAt(int i, int j)
    {
        var offsets = GetOffsets();
        Tile tile = Main.tile[i, j];

        if (tile.TileFrameX == 0)
            return new[] { offsets[0] };
        return new[] { offsets[1] };
    }
}

internal class Decor1x1Left : ModTile, IFlowerTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
        TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 0);
        TileObjectData.newTile.RandomStyleRange = 7;
        TileObjectData.newTile.StyleHorizontal = true;
        QuickTile.SetMulti(this, 1, 1, DustID.Grass, SoundID.Grass, false, new Color(161, 226, 99));

        Main.tileCut[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileNoFail[Type] = true;

        TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
        TileID.Sets.SwaysInWindBasic[Type] = true;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;
    public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (j % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipVertically;

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(12, 6), new Vector2(8, 10) };
    public bool IsFlower(int i, int j) => Main.tile[i, j].TileFrameX != 18 && Main.tile[i, j].TileFrameX != 36;

    public Vector2[] OffsetAt(int i, int j)
    {
        var offsets = GetOffsets();
        Tile tile = Main.tile[i, j];

        if (tile.TileFrameX == 0)
            return new[] { offsets[0] };
        return new[] { offsets[1] };
    }
}

internal class VerdantDecor1x1NoCut : ModTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>() };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassTypes.ToList());
        TileObjectData.newTile.RandomStyleRange = 4;
        TileObjectData.newTile.StyleHorizontal = true;
        QuickTile.SetMulti(this, 1, 1, DustID.Stone, SoundID.Dig, false, new Color(161, 226, 99));

        Main.tileCut[Type] = false;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;
    public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
}

internal class VerdantDecor2x1 : ModTile, IFlowerTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>() };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassTypes.ToList());
        TileObjectData.newTile.RandomStyleRange = 6;
        TileObjectData.newTile.StyleHorizontal = true;
        QuickTile.SetMulti(this, 2, 1, DustID.Grass, SoundID.Grass, true, new Color(161, 226, 99));

        TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
        TileID.Sets.SwaysInWindBasic[Type] = true;
    }

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(16, 8) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}

internal class VerdantDecor2x2 : ModTile, IFlowerTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>() };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassTypes.ToList());
        TileObjectData.newTile.RandomStyleRange = 8;
        TileObjectData.newTile.StyleHorizontal = true;
        QuickTile.SetMulti(this, 2, 2, DustID.Grass, SoundID.Grass, true, new Color(161, 226, 99));
    }

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(16, 16), new Vector2(7, 10), new Vector2(30, 12), new Vector2(3, 23) };

    public bool IsFlower(int i, int j)
    {
        Tile tile = Main.tile[i, j];
        return tile.TileFrameX == 180 || tile.TileFrameX == 144;
    }

    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}

internal class VerdantDecor1x2 : ModTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>() };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassTypes.ToList());
        TileObjectData.newTile.RandomStyleRange = 6;
        TileObjectData.newTile.StyleHorizontal = true;
        QuickTile.SetMulti(this, 1, 2, DustID.Grass, SoundID.Grass, true, new Color(161, 226, 99));
        Main.tileCut[Type] = true;
    }

    public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
}

internal class VerdantDecor1x3 : ModTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>() };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassTypes.ToList());
        TileObjectData.newTile.RandomStyleRange = 7;
        TileObjectData.newTile.StyleHorizontal = true;
        QuickTile.SetMulti(this, 1, 3, DustID.Grass, SoundID.Grass, true, new Color(161, 226, 99));
        Main.tileCut[Type] = true;
    }
    public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
}