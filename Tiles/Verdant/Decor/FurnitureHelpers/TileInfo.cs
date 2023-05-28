using Microsoft.Xna.Framework;

namespace Verdant.Tiles.Verdant.Decor;

public readonly struct StaticTileInfo
{
    public readonly string MapKeyName;
    public readonly int[] AdjTypes;

    public StaticTileInfo(string mapKeyName, params int[] adjTypes)
    {
        MapKeyName = mapKeyName;
        AdjTypes = adjTypes;
    }
}

public readonly struct SpecificTileInfo
{
    public readonly int DustType;
    public readonly Color MapColor;

    public SpecificTileInfo(int dustType, Color mapColor)
    {
        DustType = dustType;
        MapColor = mapColor;
    }
}