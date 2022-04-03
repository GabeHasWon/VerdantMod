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

namespace Verdant.Tiles
{
    public static class TileHelper
    {
        public static Vector2 TileOffset => Lighting.lightMode > 1 ? Vector2.Zero : Vector2.One * 12;

        public static Vector2 TileCustomPosition(int i, int j, Vector2? off = null)
        {
            return ((new Vector2(i, j) + TileOffset) * 16) - Main.screenPosition - (off ?? new Vector2(0));
        }

        public static int[] AttachStrongVine
        {
            get => new int[] { ModContent.TileType<VerdantHungTable_Pink>(), ModContent.TileType<VerdantHungTable_Red>(), ModContent.TileType<VerdantHungTable_PinkLightless>(), ModContent.TileType<VerdantHungTable_RedLightless>() };
        }

        public static bool CanGrowVerdantTree(int i, int j, int minHeight, params int[] ignoreTypes)
        {
            for (int k = j; k > j - minHeight; k--)
            {
                Tile t = Framing.GetTileSafely(i, k);
                if (ignoreTypes.Contains(t.type))
                    continue;
                if (t.active())
                    return false;
            }
            return true;
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

        public static bool ValidTop(Tile tile) => tile.active() && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) && !tile.bottomSlope() && !tile.topSlope() && !tile.halfBrick() && !tile.topSlope();
        public static bool ValidTop(int i, int j) => ValidTop(Framing.GetTileSafely(i, j));

        public static bool ValidBottom(Tile tile) => !tile.bottomSlope();
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
            Texture2D chain = ModContent.GetTexture(path);

            bool[] valids = new bool[2] { false, false };
            for (int k = j - 1; k > j - length; --k) //Woo chain drawing
            {
                if (Helper.SolidTile(i, k + 1)) valids[0] = true;
                if (Helper.SolidTile(i + 2, k + 1)) valids[1] = true;

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
            if (Helper.ActiveType(i, j - 1, ModContent.TileType<VerdantStrongVine>()) && Helper.ActiveType(i + 2, j - 1, ModContent.TileType<VerdantStrongVine>()))
                return true;
            return false;
        }
    }
}