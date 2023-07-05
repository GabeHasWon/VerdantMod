using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Plants;

class WaterberryBush : ModTile, IFlowerTile
{
    private static bool KillingStack = false;

    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.AlternateTile, 2, 0);
        TileObjectData.newTile.AnchorAlternateTiles = new int[] { Type };
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>() };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassList());
        TileObjectData.addTile(Type);

        DustType = DustID.Grass;
        HitSound = SoundID.Grass;

        Main.tileLighted[Type] = true;

        ModTranslation name = CreateMapEntryName();
        name.SetDefault("Bush");
        AddMapEntry(new Color(71, 181, 168), name);
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        float sine = MathF.Pow(MathF.Sin((Main.GameUpdateCount + (i + j) * MathHelper.Pi * 0.75f) * 0.02f), 2) * 0.7f + 0.3f;
        (r, g, b) = (0.05f * sine, 0.2f * sine, 0.5f * sine);
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (fail || KillingStack)
            return;

        Tile tile = Main.tile[i, j];
        int x = i - (tile.TileFrameX / 18);
        int y = j;

        KillingStack = true;

        while (TileHelper.ActiveType(x, y, Type))
        {
            for (int k = x; k < x + 2; ++k)
            {
                if (k == i && y == j)
                    continue;

                WorldGen.KillTile(k, y, false);
            }

            y--;
        }

        if (!noItem)
            Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, 1);

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

        tile.TileFrameY = (short)(TileHelper.ActiveType(i, j - 1, Type) ? 18 : 0);
        tile.TileFrameY += (short)(Main.rand.Next(3) * 38);
        return false;
    }

    public override void RandomUpdate(int i, int j)
    {
        if (Main.rand.NextBool(40))
        {
            int x = i - (Main.tile[i, j].TileFrameX / 18);
            WorldGen.PlaceObject(x, j - 1, Type, true);
        }
    }

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(16, 16) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}