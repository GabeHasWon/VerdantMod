using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles;

public class QuickTile
{
    /// <summary>
    /// Sets a mod tile's SetDefaults() quickly and easily. Original concept by Starlight River.
    /// </summary>
    /// <param name="t">Tile to set.</param>
    /// <param name="minPick">MinPick value for the tile.</param>
    /// <param name="dustType">Dust a tile gives off.</param>
    /// <param name="soundType">Sound a tile makes.</param>
    /// <param name="mapColor">Colour on the minimap.</param>
    /// <param name="drop">Item that the tile drops.</param>
    /// <param name="mapName">Name on the map.</param>
    public static void Set(ModTile t, int minPick, int dustType, SoundStyle soundType, Color mapColor, int drop, string mapName = "")
    {
        t.MinPick = minPick;
        t.DustType = dustType;
        t.HitSound = soundType;
        t.ItemDrop = drop;

        ModTranslation name = t.CreateMapEntryName();
        name.SetDefault(mapName);
        t.AddMapEntry(mapColor, name);
    }

    /// <summary>
    /// Sets a mod tile's Main properties easily.
    /// </summary>
    /// <param name="t">Tile to set.</param>
    /// <param name="solid">Is the tile solid?</param>
    /// <param name="mergeDirt">Does the tile merge with dirt?</param>
    /// <param name="lighted">Is the tile affected by light?</param>
    /// <param name="blockLight">Does the tile block light?</param>
    public static void SetProperties(ModTile t, bool solid, bool mergeDirt, bool lighted, bool blockLight)
    {
        Main.tileMergeDirt[t.Type] = mergeDirt;
        Main.tileSolid[t.Type] = solid;
        Main.tileLighted[t.Type] = lighted;
        Main.tileBlockLight[t.Type] = blockLight;
    }

    /// <summary>Does Set and SetProperties in one method.</summary>
    public static void SetAll(ModTile t, int minPick, int dust, SoundStyle sound, Color mapColour, int drop = 0, string mapName = "", bool solid = true, bool mergeDirt = true, bool lighted = true, bool blockLight = true)
    {
        Set(t, minPick, dust, sound, mapColour, drop, mapName);
        SetProperties(t, solid, mergeDirt, lighted, blockLight);
    }

    /// <summary>Sets a multitile's SetDefaults() quickly and easily. This DOES call TileObjectData.addTile();</summary>
    /// <param name="t">Tile to be set.</param>
    /// <param name="w">Width of the tile.</param>
    /// <param name="h">Height of the tile.</param>
    /// <param name="dust">Dust ID to give off.</param>
    /// <param name="sound">Sound ID.</param>
    /// <param name="tallBottom">A slightly taller bottom tile w/ CoordinateHeights..</param>
    /// <param name="color">Colour on the map.</param>
    /// <param name="lavaDeath">Does it die in lava?</param>
    /// <param name="topSolid">Is the top solid?</param>
    /// <param name="solid">Is the tile solid?</param>
    /// <param name="name">Name on the map.</param>
    public static void SetMulti(ModTile t, int w, int h, int dust, SoundStyle sound, bool tallBottom, Color color, bool lavaDeath = false,
        bool topSolid = false, bool solid = false, string name = null, Point16? origin = null)
    {
        Main.tileLavaDeath[t.Type] = lavaDeath;
        Main.tileFrameImportant[t.Type] = true;
        Main.tileSolidTop[t.Type] = topSolid;
        Main.tileSolid[t.Type] = solid;

        TileObjectData.newTile.Width = w;
        TileObjectData.newTile.Height = h;
        TileObjectData.newTile.CoordinateHeights = new int[h];

        for (int k = 0; k < h; k++)
            TileObjectData.newTile.CoordinateHeights[k] = 16;
        if (tallBottom)
            TileObjectData.newTile.CoordinateHeights[h - 1] = 18;

        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.newTile.Origin = origin ?? new Point16(0, 0);
        TileObjectData.addTile(t.Type);

        if (name is not null)
        {
            var mapName = t.CreateMapEntryName();
            mapName.SetDefault(name);
            t.AddMapEntry(color, mapName);
        }
        else
            t.AddMapEntry(color);

        t.DustType = dust;
        t.HitSound = sound;

        TileID.Sets.DisableSmartCursor[t.Type] = true;
    }

    public static void SetMultiLocalized(ModTile t, int w, int h, int dust, SoundStyle sound, bool tallBottom, Color color, bool lavaDeath = false,
        bool topSolid = false, bool solid = false, LocalizedText name = null, Point16? origin = null)
    {
        Main.tileLavaDeath[t.Type] = lavaDeath;
        Main.tileFrameImportant[t.Type] = true;
        Main.tileSolidTop[t.Type] = topSolid;
        Main.tileSolid[t.Type] = solid;

        TileObjectData.newTile.Width = w;
        TileObjectData.newTile.Height = h;
        TileObjectData.newTile.CoordinateHeights = new int[h];

        for (int k = 0; k < h; k++)
            TileObjectData.newTile.CoordinateHeights[k] = 16;
        if (tallBottom)
            TileObjectData.newTile.CoordinateHeights[h - 1] = 18;

        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.newTile.Origin = origin ?? new Point16(0, 0);
        TileObjectData.addTile(t.Type);

        if (name is not null)
            t.AddMapEntry(color, name);
        else
            t.AddMapEntry(color);

        t.DustType = dust;
        t.HitSound = sound;

        TileID.Sets.DisableSmartCursor[t.Type] = true;
    }

    public static void MergeWith(int self, params int[] types)
    {
        for (int i = 0; i < types.Length; ++i)
        {
            Main.tileMerge[self][types[i]] = true;
            Main.tileMerge[types[i]][self] = true;
        }
    }

    /// <summary>Sets Main.tileX[] (FrameImportant, ObsidianKill, NoFail, NoAttach) array values and TileObjectData.</summary>
    public static void CrystalAnchoringData(int type, int randomRange, int[] validAnchors = null, Action<TileObjectData> perData = null)
    {
        Main.tileFrameImportant[type] = true;
        Main.tileObsidianKill[type] = true;
        Main.tileNoFail[type] = true;

        void PerDataSets(TileObjectData data, bool resetBottomAnchor = true)
        {
            if (resetBottomAnchor)
                data.AnchorBottom = AnchorData.Empty;

            data.RandomStyleRange = randomRange;
            data.WaterDeath = false;
            data.WaterPlacement = LiquidPlacement.Allowed;
            perData?.Invoke(data);

            if (validAnchors is not null)
                data.AnchorValidTiles = validAnchors;
        }

        TileObjectData.newTile.CopyFrom(TileObjectData.StyleTorch);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
        PerDataSets(TileObjectData.newTile, false);

        TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
        TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
        TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)TileID.WoodenBeam };
        PerDataSets(TileObjectData.newAlternate);
        TileObjectData.addAlternate(1);

        TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
        TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
        TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)TileID.WoodenBeam };
        PerDataSets(TileObjectData.newAlternate);
        TileObjectData.addAlternate(2);

        TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
        TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
        PerDataSets(TileObjectData.newAlternate);
        TileObjectData.addAlternate(3);

        TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
        TileObjectData.newAlternate.AnchorWall = false;
        TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
        PerDataSets(TileObjectData.newAlternate, false);
        TileObjectData.addAlternate(0);
        TileObjectData.addTile(type);
    }
}