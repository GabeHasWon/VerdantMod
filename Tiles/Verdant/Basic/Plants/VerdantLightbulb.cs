using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Items.Verdant.Materials;
using System.Collections.Generic;
using System.Linq;

namespace Verdant.Tiles.Verdant.Basic.Plants
{
    class VerdantLightbulb : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>() };
            TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassTypes.ToList());
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.StyleHorizontal = true;

            QuickTile.SetMulti(this, 2, 2, DustID.Grass, SoundID.Grass, true, new Color(145, 7, 18));
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Lightbulb>());
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Vector2 p = new Vector2(i, j) * 16;
            float LightMult = (float)((Math.Sin((Main.GameUpdateCount + i + j) * 0.02f) * 0.8) + 0.5);

            Lighting.AddLight(p, new Vector3(0.44f, 0.17f, 0.28f) * LightMult);
            Lighting.AddLight(p, new Vector3(0.1f, 0.03f, 0.06f));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            if (Main.netMode != NetmodeID.Server)
                for (int v = 0; v < 4; ++v)
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16 + new Vector2(Main.rand.Next(54), Main.rand.Next(32)), new Vector2(0), Mod.Find<ModGore>("RedPetalFalling").Type, 1);
        }
    }
}