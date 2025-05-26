using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Verdant.Systems.RealtimeGeneration;
using Verdant.Tiles.Verdant.Basic.Mysteria;

namespace Verdant.Tiles.Verdant.Trees;

internal class MysteriaTree : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.WoodFurniture, SoundID.Dig, new Color(124, 93, 68), true, false);

        Main.tileBrick[Type] = true;
        Main.tileMergeDirt[Type] = false;
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (!fail && (TileHelper.ActiveType(i, j - 1, ModContent.TileType<MysteriaTreeTop>()) || TileHelper.ActiveType(i, j - 1, ModContent.TileType<PeaceTreeTop>())))
            WorldGen.KillTile(i, j - 1);
    }

    public static bool Generate(int x, int y, int dir = 0, UnifiedRandom random = null)
    {
        if (!GetGenerationInfo(x, y, ref dir, ref random, out int height, out int[] widths, out int index, out bool[] openSpaces))
            return false;

        for (int j = y; j > y - height; --j)
        {
            int width = index >= height - 2 ? 1 : widths[index];

            for (int i = 0; i < width; ++i)
            {
                WorldGen.PlaceTile(x + (i * dir), j, ModContent.TileType<MysteriaTree>(), true, true);
                WorldGen.PlaceTile(x + (i * dir), j + 1, ModContent.TileType<MysteriaTree>(), true, true);

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, x + (i * dir), j, 1, 2);
            }

            if (index == height - 1)
            {
                int off = 0;

                if (random.NextBool(3))
                {
                    WorldGen.PlaceTile(x, j - 1, ModContent.TileType<MysteriaTree>(), true, true);

                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendTileSquare(-1, x, j - 1);

                    off = 1;
                }

                WorldGen.PlaceTile(x, j - 1 - off, ModContent.TileType<MysteriaTreeTop>(), true, true);

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, x, j - 1 - off);
            }

            x += width * dir;
            index++;
        }
        return true;
    }

    public static bool GetGenerationInfo(int x, int y, ref int dir, ref UnifiedRandom random, out int height, out int[] widths, out int index, out bool[] openSpaces)
    {
        random ??= Main.rand;

        height = random.Next(3, 8);
        widths = [random.Next(4, 7), random.Next(3, 5), random.Next(2, 4), random.Next(1, 3), 1, 1, 1];
        index = 0;
        openSpaces = CheckOpenSpace(x, y, widths, height, random);

        if ((!openSpaces[0] && !openSpaces[1]) || (dir == -1 && !openSpaces[0]) || (dir == 1 && !openSpaces[1]))
            return false;

        dir = dir == 0 ? GetValidDirection(x, y, widths, height, openSpaces, random) : dir;
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

            static bool IsImpermeable(Tile t) => WorldGen.SolidOrSlopedTile(t) || (t.HasTile && !Main.tileCut[t.TileType] && t.TileType != ModContent.TileType<MysteriaSprout>());

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
                    Tile tile = Main.tile[modX + (i * dir), j];

                    if (IsImpermeable(tile))
                    {
                        failExit = true;
                        break;
                    }
                }

                if (failExit)
                    continue;

                if (index == height - 1)
                {
                    int off = 0;

                    if (random.NextBool(3))
                    {
                        Tile tile = Main.tile[modX, j - 1];

                        if (IsImpermeable(tile))
                        {
                            failExit = true;
                            break;
                        }
                        off = 1;
                    }

                    if (IsImpermeable(Main.tile[x, j - 1 - off]))
                    {
                        failExit = true;
                        break;
                    }
                }

                if (failExit)
                    continue;

                modX += width * dir;
                index++;
            }
        }
        return ret;
    }

    public static Queue<RealtimeStep> RealtimeGenerate(int x, int y, int dir = 0, UnifiedRandom random = null)
    {
        if (!GetGenerationInfo(x, y, ref dir, ref random, out int height, out int[] widths, out int index, out bool[] openSpaces))
            return new();

        Queue<RealtimeStep> queue = new();

        for (int j = y; j > y - height; --j)
        {
            int width = index >= height - 2 ? 1 : widths[index];

            for (int i = 0; i < width; ++i)
            {
                queue.Enqueue(new RealtimeStep(new Point16(x + (i * dir), j + 1), TileAction.PlaceTile(ModContent.TileType<MysteriaTree>(), false, true, true, sync: true)));
                queue.Enqueue(new RealtimeStep(new Point16(x + (i * dir), j), TileAction.PlaceTile(ModContent.TileType<MysteriaTree>(), false, true, true, sync: true)));
            }

            if (index == height - 1)
            {
                int off = 0;

                if (random.NextBool(3))
                {
                    queue.Enqueue(new RealtimeStep(new Point16(x, j - 1), TileAction.PlaceTile(ModContent.TileType<MysteriaTree>(), false, true, true, sync: true)));
                    off = 1;
                }

                queue.Enqueue(new RealtimeStep(new Point16(x, j - 1 - off), TileAction.PlaceTile(ModContent.TileType<MysteriaTreeTop>(), false, true, true, sync: true)));
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
