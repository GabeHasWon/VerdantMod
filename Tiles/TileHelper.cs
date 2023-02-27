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

namespace Verdant.Tiles
{
    public static class TileHelper
    {
        public static Vector2 TileOffset => Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
        public static Vector2 TileCustomPosition(int i, int j, Vector2 off = default) => (new Vector2(i, j) * 16) - Main.screenPosition - off + TileOffset;

        public static int[] AttachStrongVine
        {
            get => new int[] { ModContent.TileType<VerdantHungTable_Pink>(), ModContent.TileType<VerdantHungTable_Red>(), ModContent.TileType<VerdantHungTable_PinkLightless>(), ModContent.TileType<VerdantHungTable_RedLightless>() };
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

        public static bool ValidTop(Tile tile) => tile.HasTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]) && !tile.BottomSlope && !tile.TopSlope && !tile.IsHalfBlock && !tile.TopSlope;
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
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
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

        public static bool Spread(int i, int j, int type, int chance, params int[] validAdjacentTypes)
        {
            if (Main.rand.NextBool(chance))
            {
                var adjacents = OpenAdjacents(i, j, validAdjacentTypes);

                if (adjacents.Count == 0)
                    return false;

                Point p = adjacents[Main.rand.Next(adjacents.Count)];

                Framing.GetTileSafely(p.X, p.Y).TileType = (ushort)type;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, p.X, p.Y, 1, TileChangeType.None);
                return true;
            }
            return false;
        }

        public static List<Point> OpenAdjacents(int i, int j, params int[] types)
        {
            var p = new List<Point>();
            for (int k = -1; k < 2; ++k)
                for (int l = -1; l < 2; ++l)
                    if (!(l == 0 && k == 0) && Framing.GetTileSafely(i + k, j + l).HasTile && types.Contains(Framing.GetTileSafely(i + k, j + l).TileType))
                        p.Add(new Point(i + k, j + l));
            return p;
        }

        public static void CrystalSetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            int frameX = Main.tile[i, j].TileFrameX;
            spriteEffects = i % 2 == 0 && (frameX == 0 || frameX == 66) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }
    }
}