using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using static Terraria.WorldGen;

namespace Verdant
{
    public class Helper
    {
        public static int AnyTileRectangle(int i, int j, int w, int h)
        {
            int count = 0;
            for (int l = i; l < i + w; ++l)
                for (int k = j; k < j + h; ++k)
                    if (Framing.GetTileSafely(l, k).HasTile)
                        count++;
            return count;
        }

        public static int NoTileRectangle(int i, int j, int w, int h)
        {
            int count = 0;
            for (int l = i; l < i + w; ++l)
                for (int k = j; k < j + h; ++k)
                    if (!Framing.GetTileSafely(l, k).HasTile)
                        count++;
            return count;
        }

        public static int TileRectangle(int i, int j, int w, int h, params int[] types)
        {
            int count = 0;
            for (int l = i; l < i + w; ++l)
                for (int k = j; k < j + h; ++k)
                    if (Framing.GetTileSafely(l, k).HasTile && types.Any(x => x == Framing.GetTileSafely(l, k).TileType))
                        count++;
            return count;
        }

        public static bool AreaClear(int i, int j, int w, int h) => AnyTileRectangle(i, j, w, h) == 0;

        public static int WallRectangle(int i, int j, int w, int h)
        {
            int count = 0;
            for (int l = i; l < i + w; ++l)
                for (int k = j; k < j + h; ++k)
                    if (Main.tile[l, k].WallType != WallID.None)
                        count++;
            return count;
        }

        public static bool WalledSquare(int i, int j, int w, int h) => WallRectangle(i, j, w, h) == w * h;

        public static int WalledType(int i, int j, int w, int h, int type)
        {
            int count = 0;
            for (int l = i; l < i + w; ++l)
                for (int k = j; k < j + h; ++k)
                    if (Main.tile[l, k].WallType != WallID.None && Main.tile[l, k].WallType == type)
                        count++;
            return count;
        }

        public static bool WalledSquareType(int i, int j, int w, int h, int type) => WallRectangle(i, j, w, h) == w * h && WalledType(i, j, w, h, type) == w * h;

        public static int FindDown(Vector2 worldPos)
        {
            Point tPos = (worldPos / 16).ToPoint();
            while (!Main.tile[tPos.X, tPos.Y].HasTile || !Main.tileSolid[Main.tile[tPos.X, tPos.Y].TileType])
                tPos.Y++;
            return tPos.Y;
        }

        public static void ArmsTowardsMouse(Player p = null, Vector2? targetLoc = null)
        {
            if (p == null)
                p = Main.LocalPlayer;

            if (targetLoc == null)
                targetLoc = Main.MouseWorld;

            float radians = p.AngleTo(targetLoc.Value) + MathHelper.PiOver2;
            radians = Math.Abs(radians);

            int FrameSize = 56;
            bool WithinAngle(float target) => Math.Abs(target - radians) < MathHelper.PiOver4;

            if (WithinAngle(MathHelper.PiOver4) || radians < MathHelper.PiOver4)
                p.bodyFrame.Y = FrameSize * 2;
            else if (WithinAngle(MathHelper.PiOver2))
                p.bodyFrame.Y = FrameSize * 3;
            else if (WithinAngle(MathHelper.PiOver4 * 3))
                p.bodyFrame.Y = FrameSize * 4;
            else if (WithinAngle(MathHelper.PiOver2 * 3))
                p.bodyFrame.Y = FrameSize * 3;
        }

        public static Point MouseTile() => (Main.MouseWorld / 16f).ToPoint();
        public static Point MouseTile(Point offset) => ((Main.MouseWorld / 16f) + offset.ToVector2()).ToPoint();
        public static Point MouseTile(Vector2 offset) => ((Main.MouseWorld / 16f) + offset).ToPoint();
    }
}
