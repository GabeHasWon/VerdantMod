using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Blocks;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Tiles.Verdant.Basic.Plants
{
    internal class VerdantStrongVine : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileAxe[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileID.Sets.HousingWalls[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor); //this seems like a good idea

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.AnchorAlternateTiles = new int[] { Type, TileType<VerdantSoilGrass>() };
            TileObjectData.newTile.AnchorTop = new Terraria.DataStructures.AnchorData(AnchorType.SolidBottom | AnchorType.SolidTile | AnchorType.AlternateTile, 1, 0);
            TileObjectData.addTile(Type);

            drop = ItemType<VerdantStrongVineMaterial>();
            AddMapEntry(new Color(182, 224, 49));
            dustType = DustID.Grass;
            soundType = SoundID.Grass;
            minPick = 20;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            bool validAbove = Helper.ActiveType(i, j - 1, TileType<VerdantSoilGrass>()) || Helper.ActiveType(i, j - 1, Type);
            bool validBelow = Helper.ActiveType(i, j + 1, TileType<VerdantSoilGrass>()) || Helper.ActiveType(i, j + 1, Type);

            if (!validBelow) //Hanging table functionality
            {
                Tile below = Framing.GetTileSafely(i, j + 1);
                if (below.active() && TileHelper.AttachStrongVine.Contains(below.type))
                    validBelow = true;
            }

            if (!validAbove && validBelow)
                Framing.GetTileSafely(i, j).frameX = 36;
            else if (validAbove && !validBelow)
                Framing.GetTileSafely(i, j).frameX = 18;
            else
                Framing.GetTileSafely(i, j).frameX = 0;
            Framing.GetTileSafely(i, j).frameY = (short)(Main.rand.Next(2) * 18);

            if (!validAbove && !validBelow)
            {
                Framing.GetTileSafely(i, j).frameX = 0;
                Framing.GetTileSafely(i, j).frameY = (short)((Main.rand.Next(2) * 18) + 36);
            }
            return false;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (!Main.tile[i, j + 1].active() && Main.rand.Next(7) == 0)
                WorldGen.PlaceTile(i, j + 1, Type);

            if (Framing.GetTileSafely(i, j).frameX != 0 && Framing.GetTileSafely(i, j).frameY < 36 && Main.rand.Next(2) == 0)
                Framing.GetTileSafely(i, j).frameY = (short)((Main.rand.Next(2) * 18) + 36);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Framing.GetTileSafely(i, j).frameX == 18 && Framing.GetTileSafely(i, j).frameY == 54)
                Lighting.AddLight(new Vector2(i, j) * 16, new Vector3(0.2f, 0.06f, 0.12f));
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if ((Framing.GetTileSafely(i, j).frameX == 18 && Framing.GetTileSafely(i, j).frameY == 54 || Framing.GetTileSafely(i, j).frameX == 18 && Framing.GetTileSafely(i, j).frameY == 36))
            {
                if (!noItem) Item.NewItem(new Rectangle(i * 16, j * 16, 16, 16), ItemType<RedPetal>());
                Gore.NewGore(new Vector2(i, j) * 16 + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), new Vector2(0), mod.GetGoreSlot("Gores/Verdant/RedPetalFalling"), 1);
            }
            if ((Framing.GetTileSafely(i, j).frameX == 36 && Framing.GetTileSafely(i, j).frameY == 36 || Framing.GetTileSafely(i, j).frameX == 36 && Framing.GetTileSafely(i, j).frameY == 54))
            {
                if (!noItem) Item.NewItem(new Rectangle(i * 16, j * 16, 16, 16), ItemType<PinkPetal>());
                Gore.NewGore(new Vector2(i, j) * 16 + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), new Vector2(0), mod.GetGoreSlot("Gores/Verdant/PinkPetalFalling"), 1);
            }

            if (Main.tile[i, j + 1].type == Type)
                WorldGen.KillTile(i, j + 1, fail, false, false);
        }
    }
}