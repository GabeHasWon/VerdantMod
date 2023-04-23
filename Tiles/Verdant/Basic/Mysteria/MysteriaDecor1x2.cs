﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Mysteria
{
    internal class MysteriaDecor1x2 : ModTile, IFlowerTile
    {
        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>() };
            TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassList());
            TileObjectData.newTile.RandomStyleRange = 6;
            TileObjectData.newTile.StyleHorizontal = true;
            QuickTile.SetMulti(this, 1, 2, DustID.Grass, SoundID.Grass, true, new Color(148, 113, 207));
            Main.tileCut[Type] = true;
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        public Vector2[] GetOffsets() => new Vector2[] { new Vector2(8, 8) };
        public bool IsFlower(int i, int j) => Main.tile[i, j].TileFrameY == 0;
        public Vector2[] OffsetAt(int i, int j) => GetOffsets();
    }
}
