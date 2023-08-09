using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Plants;

internal class VerdantStrongVine : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = false;
        Main.tileBlockLight[Type] = false;
        Main.tileFrameImportant[Type] = true;
        Main.tileAxe[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
        TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom | AnchorType.AlternateTile, 1, 0);
        TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
        TileObjectData.newTile.AnchorAlternateTiles = new int[] { Type };
        TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
        TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom | AnchorType.AlternateTile, 1, 0);
        TileObjectData.newAlternate.AnchorTop = AnchorData.Empty;
        TileObjectData.addAlternate(0);
        TileObjectData.addTile(Type);

        DustType = DustID.Grass;
        HitSound = SoundID.Grass;
        MinPick = 20;

        RegisterItemDrop(ModContent.ItemType<LushLeaf>());
    }

    public override IEnumerable<Item> GetItemDrops(int i, int j)
    {
        yield return new Item(ModContent.ItemType<LushLeaf>()) { stack = Main.rand.Next(1, 4)};
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

    public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
    {
        bool validAbove = TileHelper.ActiveType(i, j - 1, ModContent.TileType<VerdantGrassLeaves>()) || 
            TileHelper.ActiveType(i, j - 1, ModContent.TileType<Bouncebloom>()) || TileHelper.ActiveType(i, j - 1, Type);
        bool validBelow = TileHelper.ActiveType(i, j + 1, ModContent.TileType<VerdantGrassLeaves>()) || TileHelper.ActiveType(i, j + 1, Type);

        if (!validBelow) //Hanging table functionality
        {
            Tile below = Framing.GetTileSafely(i, j + 1);
            if (below.HasTile && TileHelper.AttachStrongVine.Contains(below.TileType))
                validBelow = true;
        }

        if (!validAbove && validBelow)
            Framing.GetTileSafely(i, j).TileFrameX = 36;
        else if (validAbove && !validBelow)
            Framing.GetTileSafely(i, j).TileFrameX = 18;
        else
            Framing.GetTileSafely(i, j).TileFrameX = 0;
        Framing.GetTileSafely(i, j).TileFrameY = (short)(Main.rand.Next(2) * 18);

        if (!validAbove && !validBelow)
        {
            Framing.GetTileSafely(i, j).TileFrameX = 0;
            Framing.GetTileSafely(i, j).TileFrameY = (short)((Main.rand.Next(2) * 18) + 36);
        }
        return false;
    }

    public override void RandomUpdate(int i, int j)
    {
        if (!Main.tile[i, j + 1].HasTile && Main.rand.NextBool(7))
            TileHelper.SyncedPlace(i, j + 1, Type);

        if (Framing.GetTileSafely(i, j).TileFrameX != 0 && Framing.GetTileSafely(i, j).TileFrameY < 36 && Main.rand.NextBool(2))
            Framing.GetTileSafely(i, j).TileFrameY = (short)((Main.rand.Next(2) * 18) + 36);
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (Framing.GetTileSafely(i, j).TileFrameX == 18 && Framing.GetTileSafely(i, j).TileFrameY == 54)
            Lighting.AddLight(new Vector2(i, j) * 16, new Vector3(0.2f, 0.06f, 0.12f));
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (Main.tile[i, j + 1].TileType == Type)
            WorldGen.KillTile(i, j + 1, fail, false, false);
    }
}