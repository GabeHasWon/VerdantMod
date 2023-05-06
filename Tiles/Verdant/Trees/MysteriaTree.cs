using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Systems.RealtimeGeneration;

namespace Verdant.Tiles.Verdant.Trees;

internal class MysteriaTree : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.WoodFurniture, SoundID.Dig, new Color(124, 93, 68), ModContent.ItemType<MysteriaWood>(), "", true, false);

        Main.tileBlendAll[Type] = true;
        Main.tileBrick[Type] = true;
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (!fail && (TileHelper.ActiveType(i, j - 1, ModContent.TileType<MysteriaTreeTop>()) || TileHelper.ActiveType(i, j - 1, ModContent.TileType<PeaceTreeTop>())))
            WorldGen.KillTile(i, j - 1);
    }

    public override bool Slope(int i, int j) => false;

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];

        int frameX = tile.TileFrameX / 18 * 22;
        int frameY = tile.TileFrameY / 18 * 22;

        var source = new Rectangle(frameX, frameY, 20, 20);
        Color color = Lighting.GetColor(i, j, tile.IsActuated ? Color.Gray : Color.White);
        TileHelper.DrawSlopedGlowMask(i, j, TextureAssets.Tile[Type].Value, color, -new Vector2(2), source);
        return false;
    }

    public static bool Generate(int x, int y, int dir = 0, UnifiedRandom random = null)
    {
        int height = random.Next(3, 8);
        int[] widths = new int[7] { random.Next(4, 7), random.Next(3, 5), random.Next(2, 4), random.Next(1, 3), 1, 1, 1 };
        int index = 0;

        random ??= Main.rand;

        bool[] openSpaces = CheckOpenSpace(x, y, widths, height, random);

        if ((!openSpaces[0] && !openSpaces[1]) || (dir == -1 && !openSpaces[0]) || (dir == 1 && !openSpaces[1]))
            return false;

        dir = dir == 0 ? GetValidDirection(x, y, widths, height, openSpaces, random) : dir;

        for (int j = y; j > y - height; --j)
        {
            int width = index >= height - 2 ? 1 : widths[index];

            for (int i = 0; i < width; ++i)
            {
                WorldGen.PlaceTile(x + (i * dir), j, ModContent.TileType<MysteriaTree>(), true, true);
                WorldGen.PlaceTile(x + (i * dir), j + 1, ModContent.TileType<MysteriaTree>(), true, true);
            }

            if (index == height - 1)
            {
                int off = 0;

                if (random.NextBool(3))
                {
                    WorldGen.PlaceTile(x, j - 1, ModContent.TileType<MysteriaTree>(), true, true);
                    off = 1;
                }

                WorldGen.PlaceTile(x, j - 1 - off, ModContent.TileType<MysteriaTreeTop>(), true, true);
            }

            x += width * dir;
            index++;
        }
        return true;
    }

    public static bool[] CheckOpenSpace(int x, int y, int[] widths, int height, UnifiedRandom random)
    {
        bool[] ret = new bool[2] { true, true };
        bool failExit = false;

        for (int repeats = 0; repeats < 2; ++repeats)
        {
            int dir = repeats == 0 ? -1 : 1;
            int modX = x;
            int index = 0;

            for (int j = y; j > y - height; --j)
            {
                if (failExit)
                {
                    ret[repeats] = false;
                    failExit = false;
                    break;
                }

                int width = index >= height - 2 ? 1 : widths[index];

                for (int i = 0; i < width; ++i)
                {
                    if (WorldGen.SolidOrSlopedTile(modX + (i * dir), j))
                    {
                        failExit = true;
                        break;
                    }
                }

                if (index == height - 1)
                {
                    int off = 0;

                    if (random.NextBool(3))
                    {
                        if (WorldGen.SolidOrSlopedTile(modX, j - 1))
                        {
                            failExit = true;
                            break;
                        }
                        off = 1;
                    }

                    if (WorldGen.SolidOrSlopedTile(x, j - 1 - off))
                    {
                        failExit = true;
                        break;
                    }
                }

                modX += width * dir;
                index++;
            }
        }
        return ret;
    }

    public static Queue<RealtimeStep> RealtimeGenerate(int x, int y, int dir = 0, UnifiedRandom random = null)
    {
        int[] widths = new int[7] { random.Next(4, 7), random.Next(3, 5), random.Next(2, 4), random.Next(1, 3), 1, 1, 1 };
        int height = random.Next(3, 8);
        int index = 0;

        random ??= Main.rand;

        bool[] openSpaces = CheckOpenSpace(x, y, widths, height, random);

        if ((!openSpaces[0] && !openSpaces[1]) || (dir == -1 && !openSpaces[0]) || (dir == 1 && !openSpaces[1]))
            return new();

        dir = dir == 0 ? GetValidDirection(x, y, widths, height, openSpaces, random) : dir;

        Queue<RealtimeStep> queue = new();

        for (int j = y; j > y - height; --j)
        {
            int width = index >= height - 2 ? 1 : widths[index];

            for (int i = 0; i < width; ++i)
            {
                queue.Enqueue(new RealtimeStep(new Point16(x + (i * dir), j + 1), TileAction.PlaceTile(ModContent.TileType<MysteriaTree>(), false, true, true)));
                queue.Enqueue(new RealtimeStep(new Point16(x + (i * dir), j), TileAction.PlaceTile(ModContent.TileType<MysteriaTree>(), false, true, true)));
            }

            if (index == height - 1)
            {
                int off = 0;

                if (random.NextBool(3))
                {
                    queue.Enqueue(new RealtimeStep(new Point16(x, j - 1), TileAction.PlaceTile(ModContent.TileType<MysteriaTree>(), false, true, true)));
                    off = 1;
                }

                queue.Enqueue(new RealtimeStep(new Point16(x, j - 1 - off), TileAction.PlaceTile(ModContent.TileType<MysteriaTreeTop>(), false, true, true)));
            }

            x += width * dir;
            index++;
        }

        return queue;
    }

    private static int GetValidDirection(int x, int y, int[] widths, int height, bool[] openSlots, UnifiedRandom random = null)
    {
        random ??= Main.rand;
        int sum = widths.Sum();
        int[] counts = new int[2];

        for (int i = 0; i < sum; ++i)
        {
            for (int j = y; j > y - height; --j)
            {
                if (WorldGen.SolidTile(x + i, j))
                    counts[0]++;

                if (WorldGen.SolidTile(x - i, j))
                    counts[1]++;
            }
        }

        const int Range = 4;

        if (counts[0] > counts[1] - Range && counts[0] < counts[1] + Range)
        {
            if (openSlots[0] && openSlots[1])
                return random.NextBool(2) ? -1 : 1;
            else if (!openSlots[1])
                return -1;
            else
                return 1;
        }
        else if (counts[0] > counts[1] + Range && openSlots[0])
            return -1;
        return 1;
    }
}
