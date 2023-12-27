using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Decor.VerdantFurniture;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Utilities;
using Terraria.ID;
using System.Linq;

namespace Verdant.Tiles;

public static class TileHelper
{
    public static Vector2 TileOffset => Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
    public static Vector2 TileCustomPosition(int i, int j, Vector2 off = default) => (new Vector2(i, j) * 16) - Main.screenPosition - off + TileOffset;

    public static int[] AttachStrongVine
    {
        get => new int[] { ModContent.TileType<VerdantHungTable_Pink>(), ModContent.TileType<VerdantHungTable_Red>(), ModContent.TileType<VerdantHungTable_PinkLightless>(), 
            ModContent.TileType<VerdantHungTable_RedLightless>() };
    }

    public static void OpenDoorData(int type)
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
        TileObjectData.newTile.Width = 2;
        TileObjectData.newTile.Height = 3;
        TileObjectData.newTile.Origin = new Point16(0, 0);
        TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
        TileObjectData.newTile.UsesCustomCanPlace = true;
        TileObjectData.newTile.LavaDeath = true;
        TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.StyleMultiplier = 2;
        TileObjectData.newTile.StyleWrapLimit = 2;
        TileObjectData.newTile.Direction = TileObjectDirection.PlaceRight;
        TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
        TileObjectData.newAlternate.Origin = new Point16(0, 1);
        TileObjectData.addAlternate(0);
        TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
        TileObjectData.newAlternate.Origin = new Point16(0, 2);
        TileObjectData.addAlternate(0);
        TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
        TileObjectData.newAlternate.Origin = new Point16(1, 0);
        TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
        TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
        TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
        TileObjectData.addAlternate(1);
        TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
        TileObjectData.newAlternate.Origin = new Point16(1, 1);
        TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
        TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
        TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
        TileObjectData.addAlternate(1);
        TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
        TileObjectData.newAlternate.Origin = new Point16(1, 2);
        TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
        TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
        TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
        TileObjectData.addAlternate(1);
        TileObjectData.addTile(type);
    }

    public static bool ValidTop(Tile tile) => tile.HasTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]) && !tile.BottomSlope && 
        !tile.TopSlope && !tile.IsHalfBlock && !tile.TopSlope;

    public static bool ValidTop(int i, int j) => ValidTop(Framing.GetTileSafely(i, j));

    public static bool ValidBottom(Tile tile) => !tile.BottomSlope;
    public static bool ValidBottom(int i, int j) => ValidBottom(Framing.GetTileSafely(i, j));

    public static bool ValidLeft(Tile tile) => !tile.LeftSlope;
    public static bool ValidLeft(int i, int j) => ValidLeft(Framing.GetTileSafely(i, j));

    public static bool ValidRight(Tile tile) => !tile.RightSlope;
    public static bool ValidRight(int i, int j) => ValidRight(Framing.GetTileSafely(i, j));

    public static bool CanPlaceHangingTable(int i, int j)
    {
        if (ActiveType(i, j - 1, ModContent.TileType<VerdantStrongVine>()) && ActiveType(i + 2, j - 1, ModContent.TileType<VerdantStrongVine>()))
            return true;
        return false;
    }

    public static bool HasOpenAdjacent(int i, int j)
    {
        for (int l = -1; l < 1; ++l)
            for (int k = -1; k < 1; ++k)
                if (new Point(i + l, j + k) != new Point(i, j) && !Framing.GetTileSafely(i + l, j + k).HasTile)
                    return true;
        return false;
    }

    public static Point GetOpenAdjacent(int i, int j)
    {
        for (int l = -1; l < 1; ++l)
            for (int k = -1; k < 1; ++k)
                if (!Framing.GetTileSafely(i + l, j + k).HasTile && new Point(i + l, j + k) != new Point(i, j))
                    return new Point(l, k);
        return new Point(-2, -2);
    }

    public static Point GetRandomOpenAdjacent(int i, int j, UnifiedRandom rand = null)
    {
        rand = rand ?? WorldGen.genRand;

        List<Point> adjacents = new List<Point>();
        for (int l = -1; l < 1; ++l)
            for (int k = -1; k < 1; ++k)
                if (!Framing.GetTileSafely(i + l, j + k).HasTile && new Point(i + l, j + k) != new Point(i, j))
                    adjacents.Add(new Point(l, k));
        if (adjacents.Count > 0)
            return adjacents[rand.Next(adjacents.Count)];
        return new Point(-2, -2);
    }

    public static Point GetTopLeft(Point j)
    {
        var tile = Main.tile[j.X, j.Y];
        TileObjectData data = TileObjectData.GetTileData(tile);

        if (data is null)
            return j;

        (int x, int y) = (j.X, j.Y);
        x -= (int)(tile.TileFrameX / 18f) % data.Width;
        y -= (int)(tile.TileFrameY / 18f) % data.Height;
        return new Point(x, y);
    }

    public static bool SyncedPlace(int i, int j, int type, bool mute = false, bool forced = false, int plr = -1, int style = 0)
    {
        bool success = WorldGen.PlaceTile(i, j, type, mute, forced, plr, style);

        if (!success)
            return false;

        if (Main.netMode != NetmodeID.SinglePlayer)
        {
            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
            NetMessage.SendTileSquare(-1, i, j);
        }
        return true;
    }

    public static void SyncedKill(int i, int j)
    {
        WorldGen.KillTile(i, j);

        if (Main.netMode != NetmodeID.SinglePlayer)
            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
    }

    public static bool SolidTile(int i, int j) => Framing.GetTileSafely(i, j).HasTile && Main.tileSolid[Framing.GetTileSafely(i, j).TileType];
    public static bool SolidTopTile(int i, int j) => Framing.GetTileSafely(i, j).HasTile && (Main.tileSolidTop[Framing.GetTileSafely(i, j).TileType] || Main.tileSolid[Framing.GetTileSafely(i, j).TileType]);
    public static bool ActiveType(int i, int j, int t) => Framing.GetTileSafely(i, j).HasTile && Framing.GetTileSafely(i, j).TileType == t;
    public static bool ActiveType(int i, int j, params int[] t) => Framing.GetTileSafely(i, j).HasTile && t.Contains(Framing.GetTileSafely(i, j).TileType);
    public static bool SolidType(int i, int j, int t) => ActiveType(i, j, t) && Framing.GetTileSafely(i, j).HasTile;
    public static bool ActiveTypeNoTopSlope(int i, int j, int t) => Framing.GetTileSafely(i, j).HasTile && Framing.GetTileSafely(i, j).TileType == t && !Framing.GetTileSafely(i, j).TopSlope;
    public static bool ActiveTypeNoBottomSlope(int i, int j, int t) => Framing.GetTileSafely(i, j).HasTile && Framing.GetTileSafely(i, j).TileType == t && !Framing.GetTileSafely(i, j).BottomSlope;

    public static void ExpandValidAnchors(this TileObjectData data, List<int> additions, bool alternates = false)
    {
        int[] values = alternates ? data.AnchorAlternateTiles : data.AnchorValidTiles;
        List<int> newValues = new(additions);
        newValues.AddRange(values);
        int[] finalValues = newValues.ToArray();

        if (alternates)
            data.AnchorAlternateTiles = finalValues;
        else
            data.AnchorValidTiles = finalValues;
    }

    public static bool Spread(int i, int j, int type, int chance, bool requiresAir, params int[] validAdjacentTypes)
    {
        if (Main.rand.NextBool(chance))
        {
            var adjacents = OpenAdjacents(i, j, requiresAir, validAdjacentTypes);

            if (adjacents.Count == 0)
                return false;

            Point p = adjacents[Main.rand.Next(adjacents.Count)];

            Framing.GetTileSafely(p.X, p.Y).TileType = (ushort)type;
            WorldGen.SquareTileFrame(i, j, true);

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendTileSquare(-1, p.X, p.Y, TileChangeType.None);
            return true;
        }
        return false;
    }

    public static List<Point> OpenAdjacents(int i, int j, bool requiresAir, params int[] types)
    {
        var p = new List<Point>();
        for (int k = -1; k < 2; ++k)
            for (int l = -1; l < 2; ++l)
                if (!(l == 0 && k == 0) && Framing.GetTileSafely(i + k, j + l).HasTile && types.Contains(Framing.GetTileSafely(i + k, j + l).TileType))
                    if (!requiresAir || OpenToAir(i + k, j + l))
                        p.Add(new Point(i + k, j + l));

        return p;
    }

    public static bool OpenToAir(int i, int j)
    {
        for (int k = -1; k < 2; ++k)
            for (int l = -1; l < 2; ++l)
                if (!(l == 0 && k == 0) && !WorldGen.SolidOrSlopedTile(i + k, j + l))
                    return true;

        return false;
    }

    public static void CrystalSetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
    {
        int frameX = Main.tile[i, j].TileFrameX;
        spriteEffects = i % 2 == 0 && (frameX == 0 || frameX == 66) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
    }

    public static void PrintTime(double time, string additionalText = "")
    {
        string text = "AM";
        if (!Main.dayTime)
            time += 54000.0;

        time = time / 86400.0 * 24.0;
        time = time - 7.5 - 12.0;

        if (time < 0.0)
            time += 24.0;

        if (time >= 12.0)
            text = "PM";

        int intTime = (int)time;
        double deltaTime = time - intTime;
        deltaTime = (int)(deltaTime * 60.0);
        string text2 = string.Concat(deltaTime);

        if (deltaTime < 10.0)
            text2 = "0" + text2;
        if (intTime > 12)
            intTime -= 12;
        if (intTime == 0)
            intTime = 12;

        var newText = string.Concat("Time: ", intTime, ":", text2, " ", text);
        Main.NewText(newText + additionalText, 255, 240, 20);
    }

    /// <summary>
    /// Code from Vortex, thanks for slogging through vanilla to get this!
    /// </summary>
    public static void DrawSlopedGlowMask(int i, int j, Texture2D texture, Color drawColor, Vector2 positionOffset, Rectangle? frameOverride = null)
    {
        Tile tile = Main.tile[i, j];
        int frameX = tile.TileFrameX;
        int frameY = tile.TileFrameY;

        if (frameOverride is not null)
        {
            frameX = frameOverride.Value.X;
            frameY = frameOverride.Value.Y;
        }

        Point frameSize = frameOverride is not null ? frameOverride.Value.Size().ToPoint() : new Point(16, 16);
        int width = frameSize.X;
        int height = frameSize.Y;

        Vector2 location = new(i * 16, j * 16);
        Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
        Vector2 offsets = -Main.screenPosition + zero + positionOffset;
        Vector2 drawCoordinates = location + offsets;

        if ((tile.Slope == 0 && !tile.IsHalfBlock) || (Main.tileSolid[tile.TileType] && Main.tileSolidTop[tile.TileType])) //second one should be for platforms
            Main.spriteBatch.Draw(texture, drawCoordinates, new Rectangle(frameX, frameY, width, height), drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        else if (tile.IsHalfBlock)
            Main.spriteBatch.Draw(texture, new Vector2(drawCoordinates.X, drawCoordinates.Y + 8), new Rectangle(frameX, frameY, width, 8), drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        else
        {
            var slope = tile.Slope;
            Rectangle frame;
            Vector2 drawPos;

            if (slope == SlopeType.SlopeDownLeft || slope == SlopeType.SlopeDownRight)
            {
                int length;
                int height2;

                for (int a = 0; a < 8; ++a)
                {
                    if (slope == SlopeType.SlopeDownRight)
                    {
                        length = 16 - a * 2 - 2;
                        height2 = 14 - a * 2;
                    }
                    else
                    {
                        length = a * 2;
                        height2 = 14 - length;
                    }

                    frame = new Rectangle(frameX + length, frameY, 2, height2);
                    drawPos = new Vector2(i * 16 + length, j * 16 + a * 2) + offsets;
                    Main.spriteBatch.Draw(texture, drawPos, frame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                }

                frame = new Rectangle(frameX, frameY + 14, width, 2);
                drawPos = new Vector2(i * 16, j * 16 + 14) + offsets;
                Main.spriteBatch.Draw(texture, drawPos, frame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
            else
            {
                int length;
                int height2;

                for (int a = 0; a < 8; ++a)
                {
                    if (slope == SlopeType.SlopeUpLeft)
                    {
                        length = a * 2;
                        height2 = 16 - length;
                    }
                    else
                    {
                        length = 16 - a * 2 - 2;
                        height2 = 16 - a * 2;
                    }

                    frame = new Rectangle(frameX + length, frameY + 16 - height2, 2, height2);
                    drawPos = new Vector2(i * 16 + length, j * 16) + offsets;
                    Main.spriteBatch.Draw(texture, drawPos, frame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                }

                drawPos = new Vector2(i * 16, j * 16) + offsets;
                frame = new Rectangle(frameX, frameY, 16, 2);
                Main.spriteBatch.Draw(texture, drawPos, frame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
        }
    }
}