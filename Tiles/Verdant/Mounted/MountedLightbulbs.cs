using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;
using Verdant.Walls;

namespace Verdant.Tiles.Verdant.Mounted
{
    class MountedLightbulb_2x2 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoAttach[Type] = true;

            TileID.Sets.FramesOnKillWall[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.RandomStyleRange = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.newTile.AnchorBottom = default;
            TileObjectData.newTile.AnchorTop = default;
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.newTile.AnchorValidWalls = new int[] { ModContent.WallType<VerdantLeafWall_Unsafe>(), ModContent.WallType<VerdantLeafWall>(), ModContent.WallType<VerdantVineWall_Unsafe>(), WallID.GrassUnsafe, WallID.Grass };
            TileObjectData.addTile(Type);

            DustType = DustID.Grass;
            TileID.Sets.DisableSmartCursor[Type] = true;

            AddMapEntry(new Color(193, 50, 109));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 32, 32), (frameX <= 18) ? ModContent.ItemType<RedPetal>() : ModContent.ItemType<PinkPetal>(), Main.rand.Next(2, 5));
            if (Main.rand.NextBool(3))
                Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 32, 32), ModContent.ItemType<VerdantFlowerBulb>(), Main.rand.Next(1, 3));
            Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 32, 32), ModContent.ItemType<Lightbulb>(), 1);

            if (frameX % 36 == 18) i--;
            if (frameY % 36 == 18) j--;

            int l = Main.rand.Next(3, 6);
            for (int v = 0; v < l; ++v)
            {
                int t = (frameX > 18) ? Mod.Find<ModGore>("PinkPetalFalling").Type : Mod.Find<ModGore>("RedPetalFalling").Type;
                Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16 + new Vector2(Main.rand.Next(32), Main.rand.Next(32)), new Vector2(0), t, 1);
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Vector2 p = new Vector2(i, j) * 16;
            float LightMult = (float)((Math.Sin((Main.time + (i + j * 7.23f)) * 0.035f) * 0.5) + 0.4);
            Lighting.AddLight(p, new Vector3(0.44f, 0.17f, 0.28f) * LightMult);
            Lighting.AddLight(p, new Vector3(0.1f, 0.03f, 0.06f));

            if (Main.rand.NextBool(700))
            {
                int type = Mod.Find<ModGore>((Framing.GetTileSafely(i, j).TileFrameX <= 19) ? "RedPetalFalling" : "PinkPetalFalling").Type;
                Gore.NewGore(new EntitySource_TileUpdate(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), Vector2.Zero, type);
            }
        }
    }
}