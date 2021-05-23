using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture
{
    internal class VerdantHungTable_Red : ModTile
    {
        public const int ChainLength = 22;

        public override void SetDefaults()
        {
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.Origin = new Point16(0, 0);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            QuickTile.SetMulti(this, 3, 2, DustID.Grass, SoundID.Grass, false, new Color(20, 82, 39), false, false, false, "");

            Main.tileCut[Type] = false;
            Main.tileSolidTop[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;

            adjTiles = new int[] { TileID.Tables };
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 2 : 5;

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Framing.GetTileSafely(i, j).frameX == 18 && Framing.GetTileSafely(i, j).frameY == 18)
                Lighting.AddLight(new Vector2(i, j) * 16, new Vector3(0.1f, 0.03f, 0.06f) * 11);
        }

        public override bool CanPlace(int i, int j) => TileHelper.CanPlaceHangingTable(i, j);

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new Vector2(i * 16, j * 16), ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantHungTableBlock_Red>(), 1);
            for (int v = 0; v < 4; ++v)
            {
                Vector2 off = new Vector2(Main.rand.Next(54), Main.rand.Next(32));
                Gore.NewGore(new Vector2(i, j) * 16 + off, new Vector2(0), Main.rand.NextBool(2) ? mod.GetGoreSlot("Gores/Verdant/LushLeaf") : mod.GetGoreSlot("Gores/Verdant/PinkPetalFalling"), 1);
            }
        }
    }

    internal class VerdantHungTable_RedLightless : ModTile
    {
        public const int ChainLength = 22;

        public override void SetDefaults()
        {
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.Origin = new Point16(0, 0);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            QuickTile.SetMulti(this, 3, 2, DustID.Grass, SoundID.Grass, false, new Color(20, 82, 39), false, false, false, "");

            Main.tileCut[Type] = false;
            Main.tileSolidTop[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;

            adjTiles = new int[] { TileID.Tables };
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 2 : 5;

        public override bool CanPlace(int i, int j) => TileHelper.CanPlaceHangingTable(i, j);

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new Vector2(i * 16, j * 16), ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantHungTableBlock_RedLightless>(), 1);
            for (int v = 0; v < 4; ++v)
            {
                Vector2 off = new Vector2(Main.rand.Next(54), Main.rand.Next(32));
                Gore.NewGore(new Vector2(i, j) * 16 + off, new Vector2(0), Main.rand.NextBool(2) ? mod.GetGoreSlot("Gores/Verdant/LushLeaf") : mod.GetGoreSlot("Gores/Verdant/PinkPetalFalling"), 1);
            }
        }
    }

    internal class VerdantHungTable_Pink : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.Origin = new Point16(0, 0);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            QuickTile.SetMulti(this, 3, 2, DustID.Grass, SoundID.Grass, false, new Color(20, 82, 39), false, false, false, "");

            Main.tileCut[Type] = false;
            Main.tileSolidTop[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;

            adjTiles = new int[] { TileID.Tables };
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 2 : 5;

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Framing.GetTileSafely(i, j).frameX == 18 && Framing.GetTileSafely(i, j).frameY == 18)
                Lighting.AddLight(new Vector2(i, j) * 16, new Vector3(0.1f, 0.03f, 0.06f) * 11);
        }

        public override bool CanPlace(int i, int j) => TileHelper.CanPlaceHangingTable(i, j);

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new Vector2(i * 16, j * 16), ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantHungTableBlock_Pink>(), 1);
            for (int v = 0; v < 4; ++v)
            {
                Vector2 off = new Vector2(Main.rand.Next(54), Main.rand.Next(32));
                Gore.NewGore(new Vector2(i, j) * 16 + off, new Vector2(0), Main.rand.NextBool(2) ? mod.GetGoreSlot("Gores/Verdant/LushLeaf") : mod.GetGoreSlot("Gores/Verdant/PinkPetalFalling"), 1);
            }
        }
    }

    internal class VerdantHungTable_PinkLightless : ModTile
    {
        public const int ChainLength = 22;

        public override void SetDefaults()
        {
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.Origin = new Point16(0, 0);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            QuickTile.SetMulti(this, 3, 2, DustID.Grass, SoundID.Grass, false, new Color(20, 82, 39), false, false, false, "");

            Main.tileCut[Type] = false;
            Main.tileSolidTop[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;

            adjTiles = new int[] { TileID.Tables };
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 2 : 5;

        public override bool CanPlace(int i, int j) => TileHelper.CanPlaceHangingTable(i, j);

        public override void PlaceInWorld(int i, int j, Item item)
        {
            WorldGen.TileFrame(i + 2, j - 1);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new Vector2(i * 16, j * 16), ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantHungTableBlock_PinkLightless>(), 1);
            for (int v = 0; v < 4; ++v)
            {
                Vector2 off = new Vector2(Main.rand.Next(54), Main.rand.Next(32));
                Gore.NewGore(new Vector2(i, j) * 16 + off, new Vector2(0), Main.rand.NextBool(2) ? mod.GetGoreSlot("Gores/Verdant/LushLeaf") : mod.GetGoreSlot("Gores/Verdant/PinkPetalFalling"), 1);
            }
        }
    }
}