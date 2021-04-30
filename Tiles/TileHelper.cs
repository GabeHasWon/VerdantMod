using Terraria;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace Verdant.Tiles
{
    public static class TileHelper
    {
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

        public static bool ValidTop(Tile tile) => tile.active() && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) && !tile.bottomSlope() && !tile.topSlope() && !tile.halfBrick() && !tile.topSlope();
        public static bool ValidTop(int i, int j) => ValidTop(Framing.GetTileSafely(i, j));

        public static bool ValidBottom(Tile tile) => !tile.bottomSlope();
        public static bool ValidBottom(int i, int j) => ValidBottom(Framing.GetTileSafely(i, j));

        public static bool DrawChains(int i, int j, string path, SpriteBatch b, int length)
        {
            Texture2D chain = GetTexture(path);

            bool[] valids = new bool[2] { false, false };
            for (int k = j - 1; k > j - length; --k) //Woo chain drawing
            {
                if (Helper.SolidTile(i, k + 1)) valids[0] = true;
                if (Helper.SolidTile(i + 2, k + 1)) valids[1] = true;

                float offset = (float)Math.Sin((Main.time + (i * 24) + (k * 19)) * (0.02f * (!Lighting.NotRetro ? 0f : 1))) * 1.2f;

                if (k == j - 1) offset *= 0.5f;

                if (!valids[0])
                    b.Draw(chain, Helper.TileCustomPosition(i, k, new Vector2(offset, 0)), new Rectangle(0, 0, 16, 16), Lighting.GetColor(i, k), 0f, new Vector2(), 1f, SpriteEffects.None, 1f);
                if (!valids[1])
                    b.Draw(chain, Helper.TileCustomPosition(i + 2, k, new Vector2(offset, 0)), new Rectangle(0, 0, 16, 16), Lighting.GetColor(i + 2, k), 0f, new Vector2(), 1f, SpriteEffects.None, 1f);

                if (valids[0] && valids[1])
                    return false;
            }
            return true;
        }

        public static bool CanPlaceHangingTable(int i, int j, int length)
        {
            bool[] valids = new bool[2] { false, false };
            for (int k = j; k > j - length; --k) //Can only be placed with valid & within-distance anchor
            {
                if (Helper.SolidTile(i, k)) valids[0] = true;
                if (Helper.SolidTile(i + 2, k)) valids[1] = true;

                if (valids[0] && valids[1]) return true;
            }
            return false;
        }
    }
}