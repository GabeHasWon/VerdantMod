using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Decor.VerdantFurniture;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Utilities;

namespace Verdant.Tiles
{
    public static class TileHelper
    {
        public static Vector2 TileOffset => Lighting.LegacyEngine.Mode > 1 ? Vector2.Zero : Vector2.One * 12;

        public static Vector2 TileCustomPosition(int i, int j, Vector2? off = null)
        {
            return ((new Vector2(i, j) + TileOffset) * 16) - Main.screenPosition - (off ?? new Vector2(0));
        }

        public static int[] AttachStrongVine
        {
            get => new int[] { ModContent.TileType<VerdantHungTable_Pink>(), ModContent.TileType<VerdantHungTable_Red>(), ModContent.TileType<VerdantHungTable_PinkLightless>(), ModContent.TileType<VerdantHungTable_RedLightless>() };
        }

        public static void OpenDoorData(int type)
        {
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

        /// <summary>
        /// Don't use the bool return value for anything.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="path"></param>
        /// <param name="b"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool DrawChains(int i, int j, string path, SpriteBatch b, int length)
        {
            Texture2D chain = ModContent.Request<Texture2D>(path).Value;

            bool[] valids = new bool[2] { false, false };
            for (int k = j - 1; k > j - length; --k) //Woo chain drawing
            {
                if (SolidTile(i, k + 1)) valids[0] = true;
                if (SolidTile(i + 2, k + 1)) valids[1] = true;

                float offset = (float)Math.Sin((Main.time + (i * 24) + (k * 19)) * (0.02f * (!Lighting.NotRetro ? 0f : 1))) * 1.2f;

                if (k == j - 1) offset *= 0.5f;

                offset = 0f;

                if (!valids[0])
                    b.Draw(chain, TileCustomPosition(i, k, new Vector2(offset, 0)), new Rectangle(0, 0, 16, 16), Lighting.GetColor(i, k), 0f, new Vector2(), 1f, SpriteEffects.None, 1f);
                if (!valids[1])
                    b.Draw(chain, TileCustomPosition(i + 2, k, new Vector2(offset, 0)), new Rectangle(0, 0, 16, 16), Lighting.GetColor(i + 2, k), 0f, new Vector2(), 1f, SpriteEffects.None, 1f);

                if (valids[0] && valids[1])
                    return false;
            }
            return true;
        }

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
                return new Point(-1, -1);

            (int x, int y) = (j.X, j.Y);
            x -= (int)(tile.TileFrameX / 18f) % data.Width;
            y -= (int)(tile.TileFrameY / 18f) % data.Height;
            return new Point(x, y);
        }

        public static bool SolidTile(int i, int j) => Framing.GetTileSafely(i, j).HasTile && Main.tileSolid[Framing.GetTileSafely(i, j).TileType];
        public static bool SolidTopTile(int i, int j) => Framing.GetTileSafely(i, j).HasTile && (Main.tileSolidTop[Framing.GetTileSafely(i, j).TileType] || Main.tileSolid[Framing.GetTileSafely(i, j).TileType]);
        public static bool ActiveType(int i, int j, int t) => Framing.GetTileSafely(i, j).HasTile && Framing.GetTileSafely(i, j).TileType == t;
        public static bool SolidType(int i, int j, int t) => ActiveType(i, j, t) && Framing.GetTileSafely(i, j).HasTile;
        public static bool ActiveTypeNoTopSlope(int i, int j, int t) => Framing.GetTileSafely(i, j).HasTile && Framing.GetTileSafely(i, j).TileType == t && !Framing.GetTileSafely(i, j).TopSlope;
    }
}