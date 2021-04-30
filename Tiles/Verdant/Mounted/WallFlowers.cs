using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks;
using Verdant.Items.Verdant.Materials;
using Verdant.Walls.Verdant;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Tiles.Verdant.Mounted
{
    class Flower_2x2 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true; //Does NOT need tile-based anchor
            Main.tileLavaDeath[Type] = true;
            Main.tileNoAttach[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.RandomStyleRange = 4;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.newTile.AnchorValidWalls = new int[] { WallType<VerdantLeafWall_Unsafe>(), WallType<VerdantLeafWall>(), WallType<VerdantVineWall_Unsafe>(), WallID.GrassUnsafe, WallID.Grass };
            TileObjectData.addTile(Type);

            dustType = DustID.Grass;
            disableSmartCursor = true;
            AddMapEntry(new Color(193, 50, 109));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new Rectangle(i * 16, j * 16, 32, 32), (frameX <= 18) ? ItemType<RedPetal>() : ItemType<PinkPetal>(), Main.rand.Next(2, 5));
            if (Main.rand.Next(3) == 0)
                Item.NewItem(new Rectangle(i * 16, j * 16, 32, 32), ItemType<VerdantFlowerBulb>(), Main.rand.Next(1, 3));

            if (frameX % 36 == 18) i--;
            if (frameY % 36 == 18) j--;

            int l = Main.rand.Next(3, 6);
            for (int v = 0; v < l; ++v)
            {
                int t = (frameX > 18) ? mod.GetGoreSlot("Gores/Verdant/PinkPetalFalling") : mod.GetGoreSlot("Gores/Verdant/RedPetalFalling");
                Gore.NewGore(new Vector2(i, j) * 16 + new Vector2(Main.rand.Next(32), Main.rand.Next(32)), new Vector2(0), t, 1);
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Main.rand.Next(700) == 0)
                Gore.NewGore((new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), Vector2.Zero, mod.GetGoreSlot((Framing.GetTileSafely(i, j).frameX <= 19) ? "Gores/Verdant/RedPetalFalling" : "Gores/Verdant/PinkPetalFalling"));
        }
    }

    class Flower_3x3 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileNoAttach[Type] = true; //Does NOT need tile-based anchor
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.RandomStyleRange = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.newTile.AnchorBottom = default;
            TileObjectData.newTile.AnchorTop = default;
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.newTile.AnchorValidWalls = new int[] { WallType<VerdantLeafWall_Unsafe>(), WallType<VerdantLeafWall>(), WallType<VerdantVineWall_Unsafe>(), WallID.GrassUnsafe, WallID.Grass };
            TileObjectData.addTile(Type);

            dustType = DustID.Grass;
            disableSmartCursor = true;
            AddMapEntry(new Color(193, 50, 109));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int frame = frameX % 56 == 0 ? frameX / 56 : 0;
            int r = Main.rand.Next(6, 10);
            if (frame == 0)
            {
                Item.NewItem(new Rectangle(i * 16, j * 16, 54, 54), ItemType<RedPetal>(), Main.rand.Next(3, 7));
                for (int v = 0; v < r; ++v)
                    Gore.NewGore(new Vector2(i, j) * 16 + new Vector2(Main.rand.Next(54), Main.rand.Next(54)), new Vector2(0), mod.GetGoreSlot("Gores/Verdant/RedPetalFalling"), 1);
            }
            else
            {
                Item.NewItem(new Rectangle(i * 16, j * 16, 54, 54), ItemType<PinkPetal>(), Main.rand.Next(3, 7));
                for (int v = 0; v < r; ++v)
                    Gore.NewGore(new Vector2(i, j) * 16 + new Vector2(Main.rand.Next(54), Main.rand.Next(54)), new Vector2(0), mod.GetGoreSlot("Gores/Verdant/PinkPetalFalling"), 1);
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            int frame = Framing.GetTileSafely(i, j).frameX % 56 == 0 ? Framing.GetTileSafely(i, j).frameX / 56 : 0;
            if (Main.rand.Next(800) == 0)
                Gore.NewGore((new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), Vector2.Zero, mod.GetGoreSlot((frame == 0) ? "Gores/Verdant/RedPetalFalling" : "Gores/Verdant/PinkPetalFalling"));
        }
    }
}
