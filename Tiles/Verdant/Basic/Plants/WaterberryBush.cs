using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Food;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Plants;

class WaterberryBush : ModTile, IFlowerTile
{
    private static bool KillingStack = false;

    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.AlternateTile, 2, 0);
        TileObjectData.newTile.AnchorAlternateTiles = new int[] { ModContent.TileType<WaterberryBushPicked>(), ModContent.TileType<WaterberryBush>() };
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>() };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassTypes.ToList());
        TileObjectData.addTile(Type);

        DustType = DustID.Grass;
        HitSound = SoundID.Grass;

        Main.tileLighted[Type] = true;

        LocalizedText name = CreateMapEntryName();
        AddMapEntry(new Color(71, 181, 168), name);
        RegisterItemDrop(ModContent.ItemType<WaterberryBushItem>());
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        float sine = MathF.Pow(MathF.Sin((Main.GameUpdateCount + (i + j) * MathHelper.Pi * 0.75f) * 0.02f), 2) * 0.7f + 0.3f;
        (r, g, b) = (0.05f * sine, 0.2f * sine, 0.5f * sine);
    }

    public override void MouseOver(int i, int j)
    {
        Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<Waterberry>();
        Main.LocalPlayer.noThrow = 2;
        Main.LocalPlayer.cursorItemIconEnabled = true;
    }

    public override bool RightClick(int i, int j)
    {
        int adjI = i - (Main.tile[i, j].TileFrameX / 18);

        for (int k = 0; k < 2; ++k)
        {
            Tile tile = Main.tile[k + adjI, j];
            tile.TileType = (ushort)ModContent.TileType<WaterberryBushPicked>();
            Item.NewItem(new EntitySource_TileBreak(k + adjI, j), new Vector2(k + adjI, j) * 16, ModContent.ItemType<Waterberry>());
        }

        NetMessage.SendTileSquare(-1, adjI, j, 2, 1, TileChangeType.None);
        return true;
    }

    public override IEnumerable<Item> GetItemDrops(int i, int j) // This straight up doesn't work unless I call it btw
    {
        yield return new Item(ModContent.ItemType<WaterberryBushItem>());

        if (Main.tile[i, j].TileFrameX != 0)
            yield break;

        if (Main.tile[i, j].TileType == ModContent.TileType<WaterberryBush>())
            yield return new Item(ModContent.ItemType<Waterberry>()) { stack = 2 };
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (fail)
            return;

        Tile tile = Main.tile[i, j];
        int x = i - (tile.TileFrameX / 18);
        int y = j;

        if (KillingStack)
            return;

        KillingStack = true;

        while (TileHelper.ActiveType(x, y, Type))
        {
            for (int k = x; k < x + 2; ++k)
            {
                if (k == i && y == j)
                    continue;

                WorldGen.KillTile(k, y, false);
            }

            var drops = GetItemDrops(i, j); // Nonsense workaround (?? why does this need to be here)
            foreach (Item item in drops)
            {
                item.Prefix(-1);
                int num = Item.NewItem(WorldGen.GetItemSource_FromTileBreak(x, y), x * 16, y * 16, 16, 16, item);
                Main.item[num].TryCombiningIntoNearbyItems(num);
            }

            y--;
        }

        KillingStack = false;
    }

    public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
    {
        TileObjectData data = TileObjectData.GetTileData(Type, 0);
        if (!Main.tile[i, j + 1].HasTile || (!data.AnchorAlternateTiles.Contains(Main.tile[i, j + 1].TileType) && !data.AnchorValidTiles.Contains(Main.tile[i, j + 1].TileType)))
        {
            WorldGen.KillTile(i, j, false, false);
            return false;
        }

        Tile tile = Main.tile[i, j];
        tile.TileFrameX = 0;

        if (TileHelper.ActiveType(i - 1, j, Type) && Main.tile[i - 1, j].TileFrameX == 0)
            tile.TileFrameX = 18;

        tile.TileFrameY = (short)(TileHelper.ActiveType(i, j - 1, ModContent.TileType<WaterberryBush>(), ModContent.TileType<WaterberryBushPicked>()) ? 18 : 0);
        tile.TileFrameY += (short)(Main.rand.Next(3) * 38);
        return false;
    }

    public override void RandomUpdate(int i, int j)
    {
        static bool WaterAt(int x, int y) => Main.tile[x, y].LiquidType == LiquidID.Water && Main.tile[x, y].LiquidAmount > 150;

        if (WaterAt(i, j) && WaterAt(i + 1, j) && Main.rand.NextBool(40))
        {
            int x = i - (Main.tile[i, j].TileFrameX / 18);
            WorldGen.PlaceObject(x, j - 1, ModContent.TileType<WaterberryBushPicked>(), true);
        }
    }

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(16, 16) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}

internal class WaterberryBushPicked : WaterberryBush
{
    public override bool RightClick(int i, int j) => false;
    public override void MouseOver(int i, int j) { }

    public override void RandomUpdate(int i, int j)
    {
        base.RandomUpdate(i, j);

        if (Main.rand.NextBool(40))
        {
            int adjI = i - (Main.tile[i, j].TileFrameX / 18);

            for (int k = 0; k < 2; ++k)
            {
                Tile tile = Main.tile[k + adjI, j];
                tile.TileType = (ushort)ModContent.TileType<WaterberryBush>();
            }

            NetMessage.SendTileSquare(-1, adjI, j, 2, 1, TileChangeType.None);
        }
    }
}