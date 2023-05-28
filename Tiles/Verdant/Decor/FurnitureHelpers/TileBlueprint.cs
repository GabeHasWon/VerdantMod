using Terraria.Localization;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Decor;

public abstract class TileBlueprint<T> : ModTile where T : ModItem
{
    protected abstract StaticTileInfo StaticInfo { get; }
    protected abstract SpecificTileInfo SpecificInfo { get; }

    public sealed override void SetStaticDefaults()
    {
        Defaults();
        AddMapEntry(SpecificInfo.MapColor, Language.GetText(StaticInfo.MapKeyName));

        AdjTiles = StaticInfo.AdjTypes;
        DustType = SpecificInfo.DustType;
    }

    public abstract void Defaults();
}
