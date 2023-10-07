using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles;

public abstract class OmnidirectionalAnchorTile : ModTile
{
    protected virtual int StyleRange => 1;
    protected virtual int[] ValidTiles => null;

    public sealed override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileObsidianKill[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
        TileObjectData.newTile.UsesCustomCanPlace = true;
        TileObjectData.newTile.AnchorBottom = Terraria.DataStructures.AnchorData.Empty;
        TileObjectData.newTile.AnchorWall = false;
        TileObjectData.addTile(Type);

        StaticDefaults();
    }

    protected virtual void StaticDefaults() { }

    public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) => TileHelper.CrystalSetSpriteEffects(i, j, ref spriteEffects);
    public override bool CanPlace(int i, int j) => AnyValidDirection(i, j);

    public sealed override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
    {
        Tile tile = Main.tile[i, j];

        if (!AnyValidDirection(i, j, out bool left, out bool right, out bool top, out bool bottom))
        {
            WorldGen.KillTile(i, j);
            return false;
        }
        else
        {
            const int SquareSize = 18;

            if (bottom)
                tile.TileFrameX = 0;
            else if (top)
                tile.TileFrameX = SquareSize * 3;
            else if (left)
                tile.TileFrameX = SquareSize;
            else if (right)
                tile.TileFrameX = SquareSize * 2;

            tile.TileFrameY = (short)(SquareSize * Main.rand.Next(StyleRange));
        }

        return false;
    }

    private bool AnyValidDirection(int i, int j, out bool left, out bool right, out bool top, out bool bottom)
    {
        bool Valid(int x, int y)
        {
            if (ValidTiles is null)
                return WorldGen.SolidTile(x, y, true);

            return WorldGen.SolidTile(x, y, true) && ValidTiles.Contains(Main.tile[x, y].TileType);
        }

        left = Valid(i - 1, j);
        right = Valid(i + 1, j);
        top = Valid(i, j - 1);
        bottom = Valid(i, j + 1);

        return left || right || top || bottom;
    }

    private bool AnyValidDirection(int i, int j) => AnyValidDirection(i, j, out bool _, out bool _, out bool _, out bool _);
}
