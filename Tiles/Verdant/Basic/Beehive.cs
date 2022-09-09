using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.NPCs.Passive;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic;

class Beehive : ModTile
{
    public const int FrameHeight = 38;

    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>() };
        TileObjectData.newTile.StyleHorizontal = true;

        QuickTile.SetMulti(this, 2, 2, DustID.Bee, SoundID.Dig, true, new Color(232, 167, 74), false, false, false, "Beehive");
    }

    public override void HitWire(int i, int j)
    {
        Tile t = Main.tile[i, j];

        if (t.TileFrameY < FrameHeight)
            return;

        if (t.TileFrameY < FrameHeight * 2 && false)
            WorldGen.PlaceLiquid(i, j, LiquidID.Honey, 128);
        else
            WorldGen.PlaceLiquid(i, j, LiquidID.Honey, 255);

        Point tL = TileHelper.GetTopLeft(new Point(i, j));
        ResetFrame(tL);
    }

    public override bool RightClick(int i, int j)
    {
        Tile t = Main.tile[i, j];

        if (t.TileFrameY < FrameHeight)
            return false;

        HitWire(i, j);
        return true;
    }

    private static void ResetFrame(Point tL)
    {
        for (int i = tL.X; i < tL.X + 2; ++i)
        {
            for (int j = tL.Y; j < tL.Y + 2; ++j)
            {
                Tile t = Main.tile[i, j];
                t.TileFrameY = (short)(j == tL.Y ? 0 : 18);
            }
        }
    }

    internal static void IncreaseFrame(Point tL)
    {
        if (Main.tile[tL.X, tL.Y].TileFrameY >= FrameHeight * 2)
            return;

        for (int i = tL.X; i < tL.X + 2; ++i)
        {
            for (int j = tL.Y; j < tL.Y + 2; ++j)
            {
                Tile t = Main.tile[i, j];
                t.TileFrameY += FrameHeight;
            }
        }
    }

    public override void RandomUpdate(int i, int j)
    {
        if (Main.rand.NextBool(3) && BeehiveSystem.TryGet(i, j, out int count) && count < 3)
        {
            NPC.NewNPC(new EntitySource_TileUpdate(i, j), i * 16, j * 16, ModContent.NPCType<Bumblebee>());
            BeehiveSystem.Add(i, j);
        }
    }
}

public class BeehiveSystem : ModSystem
{
    public Dictionary<Point, int> BeehiveCounts = new Dictionary<Point, int>();

    public static void Add(Point point)
    {
        if (!ModContent.GetInstance<BeehiveSystem>().BeehiveCounts.ContainsKey(point))
            ModContent.GetInstance<BeehiveSystem>().BeehiveCounts.Add(point, 0);

        ModContent.GetInstance<BeehiveSystem>().BeehiveCounts[point]++;
    }

    public static void Add(int i, int j) => Add(new Point(i, j));

    public static void Remove(Point point)
    {
        if (!ModContent.GetInstance<BeehiveSystem>().BeehiveCounts.ContainsKey(point))
            return;

        ModContent.GetInstance<BeehiveSystem>().BeehiveCounts[point]--;

        if (ModContent.GetInstance<BeehiveSystem>().BeehiveCounts[point] <= 0)
            ModContent.GetInstance<BeehiveSystem>().BeehiveCounts.Remove(point);
    }

    public static void Remove(int i, int j) => Remove(new Point(i, j));

    public static bool TryGet(Point point, out int value)
    {
        if (ModContent.GetInstance<BeehiveSystem>().BeehiveCounts.ContainsKey(point))
        {
            value = ModContent.GetInstance<BeehiveSystem>().BeehiveCounts[point];
            return true;
        }
        value = 0;
        return true;
    }

    public static bool TryGet(int i, int j, out int value) => TryGet(new Point(i, j), out value);
}