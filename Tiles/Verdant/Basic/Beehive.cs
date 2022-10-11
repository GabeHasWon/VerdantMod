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
    public const int MaxFrame = 4;

    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>(), TileID.HallowedGrass, TileID.Grass, TileID.JungleGrass, TileID.Hive };

        QuickTile.SetMulti(this, 2, 2, DustID.Bee, SoundID.Dig, true, new Color(232, 167, 74), true, false, false, "Beehive");
    }

    public override void HitWire(int i, int j)
    {
        Tile t = Main.tile[i, j];

        if (t.TileFrameY < FrameHeight)
            return;

        if (t.TileFrameY < FrameHeight * 2)
            WorldGen.PlaceLiquid(i, j, LiquidID.Honey, 128);
        else if (t.TileFrameY < FrameHeight * 3)
            WorldGen.PlaceLiquid(i, j, LiquidID.Honey, 255);
        else if (t.TileFrameY < FrameHeight * 4)
        {
            WorldGen.PlaceLiquid(i, j, LiquidID.Honey, 255);
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Verdant.Food.HoneyNuggets>(), Main.rand.Next(2) + 1);
        }
        else
        {
            WorldGen.PlaceLiquid(i, j, LiquidID.Honey, 255);
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Verdant.Food.HoneyNuggets>(), Main.rand.Next(2) + 2);
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Hive, Main.rand.Next(2) + 1);
        }

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
        if (Main.tile[tL.X, tL.Y].TileFrameY >= FrameHeight * MaxFrame)
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
        if (Main.rand.NextBool(1) && BeehiveSystem.TryGet(i, j, out int count) && count < 3)
        {
            NPC.NewNPC(new EntitySource_TileUpdate(i, j), i * 16, j * 16, ModContent.NPCType<Bumblebee>());
            BeehiveSystem.Add(i, j);
        }
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Verdant.Blocks.BeehiveBlock>());
}

public class BeehiveSystem : ModSystem
{
    public Dictionary<Point, int> BeehiveCounts = new();

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