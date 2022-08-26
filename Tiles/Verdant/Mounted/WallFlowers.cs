using Microsoft.Xna.Framework;
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
    class Flower_2x2 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoAttach[Type] = true;

            TileID.Sets.FramesOnKillWall[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.RandomStyleRange = 4;
            TileObjectData.newTile.StyleHorizontal = true;
            //TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.newTile.AnchorValidWalls = new int[] { ModContent.WallType<VerdantLeafWall_Unsafe>(), ModContent.WallType<VerdantLeafWall>(), ModContent.WallType<VerdantVineWall_Unsafe>(), WallID.GrassUnsafe, WallID.Grass };
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.AnchorTop = AnchorData.Empty;
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

            if (frameX == 18) i--;
            if (frameY == 18) j--;

            int l = Main.rand.Next(3, 6);
            for (int v = 0; v < l; ++v)
            {
                int t = (frameX > 18) ? Mod.Find<ModGore>("PinkPetalFalling").Type : Mod.Find<ModGore>("RedPetalFalling").Type;
                Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16 + new Vector2(Main.rand.Next(32), Main.rand.Next(32)), new Vector2(0), t, 1);
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Main.rand.NextBool(700))
                Gore.NewGore(new EntitySource_TileBreak(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), Vector2.Zero, Mod.Find<ModGore>((Framing.GetTileSafely(i, j).TileFrameX <= 19) ? "RedPetalFalling" : "PinkPetalFalling").Type);
        }
    }

    class Flower_3x3 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileID.Sets.FramesOnKillWall[Type] = true; //Kill on wall kill

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
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
            int frame = frameX % 54 == 0 ? frameX / 54 : 0;
            int r = Main.rand.Next(6, 10);
            if (frame == 0)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 54, 54), ModContent.ItemType<RedPetal>(), Main.rand.Next(3, 7));
                for (int v = 0; v < r; ++v)
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16 + new Vector2(Main.rand.Next(54), Main.rand.Next(54)), new Vector2(0), Mod.Find<ModGore>("RedPetalFalling").Type, 1);
            }
            else
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 54, 54), ModContent.ItemType<PinkPetal>(), Main.rand.Next(3, 7));
                for (int v = 0; v < r; ++v)
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16 + new Vector2(Main.rand.Next(54), Main.rand.Next(54)), new Vector2(0), Mod.Find<ModGore>("PinkPetalFalling").Type, 1);
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            int frame = Framing.GetTileSafely(i, j).TileFrameX % 56 == 0 ? Framing.GetTileSafely(i, j).TileFrameX / 56 : 0;
            if (Main.rand.NextBool(800))
                Gore.NewGore(new EntitySource_TileBreak(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), Vector2.Zero, Mod.Find<ModGore>((frame == 0) ? "RedPetalFalling" : "PinkPetalFalling").Type);
        }
    }
}
