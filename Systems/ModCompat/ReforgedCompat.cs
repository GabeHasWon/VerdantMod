using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Decor;

using MapFunc = System.Func<int, int, System.Collections.Generic.HashSet<string>, bool>;

namespace Verdant.Systems.ModCompat;

internal class ReforgedCompat : ILoadable
{
    void ILoadable.Load(Mod mod)
    {
        if (!ModLoader.TryGetMod("SpiritReforged", out Mod reforged))
            return;

        reforged.Call("RegisterPin", mod, "ApotheosisPin", "Verdant/Systems/ModCompat/ApotheosisPin");
        reforged.Call("RegisterInterest", mod, "ApotheosisInterest", (Func<int[]>)(() => [ModContent.Find<ModItem>("Verdant/ApotheosisPin").Type]), false, null, null, null);

        reforged.Call("RegisterPin", mod, "SnailPin", "Verdant/Systems/ModCompat/SnailPin");
        reforged.Call("RegisterInterest", mod, "SnailInterest", (Func<int[]>)(() => [ModContent.Find<ModItem>("Verdant/SnailPin").Type]), true, (MapFunc)MapSnailStatue, null, null);
    }

    private static bool MapSnailStatue(int x, int y, HashSet<string> hasBeenPlaced)
    {
        Tile tile = Main.tile[x, y];
        return tile.HasTile && tile.TileType == ModContent.TileType<SnailStatue>() && !hasBeenPlaced.Contains("Verdant/SnailInterest");
    }

    void ILoadable.Unload()
    {
    }
}
