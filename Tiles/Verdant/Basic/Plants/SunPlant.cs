using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Plants;

class SunPlant : ModTile, IFlowerTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>(), TileID.Dirt };

        List<int> grasses = new(VerdantGrassLeaves.VerdantGrassList());

        for (int i = 0; i < TileID.Sets.Grass.Length; ++i)
            if (TileID.Sets.Grass[i])
                grasses.Add(i);

        TileObjectData.newTile.ExpandValidAnchors(grasses);
        TileObjectData.newTile.RandomStyleRange = 1;
        TileObjectData.newTile.StyleHorizontal = true;

        QuickTile.SetMulti(this, 2, 2, DustID.Grass, SoundID.Grass, true, new Color(143, 21, 193));
    }

    public override void NearbyEffects(int i, int j, bool closer) => Lighting.AddLight(Main.LocalPlayer.Center, (Color.Yellow * 20).ToVector3());

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(16, 16) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}