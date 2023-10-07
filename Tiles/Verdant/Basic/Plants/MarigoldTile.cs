﻿using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Tiles.TileEntities.Verdant;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Plants;

class MarigoldTile : ModTile, IFlowerTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>() };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassTypes.ToList());
        TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);

        QuickTile.SetMulti(this, 2, 2, DustID.Grass, SoundID.Grass, true, new Color(242, 207, 82), true, false, false, "Marigold");
    }

    public override void PlaceInWorld(int i, int j, Item item) => ModContent.GetInstance<MarigoldTE>().Place(i, j);
    public override void KillMultiTile(int i, int j, int frameX, int frameY) => ModContent.GetInstance<MarigoldTE>().Kill(i, j); 

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(16, 13) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();

    public bool OnPollenate(int i, int j)
    {
        (TileEntity.ByPosition[new Point16(i, j)] as MarigoldTE).coinTimes.Add(MarigoldTE.CoinTimeMax);
        return true;
    }
}