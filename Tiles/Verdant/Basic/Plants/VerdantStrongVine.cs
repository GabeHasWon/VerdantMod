using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Plants
{
    internal class VerdantStrongVine : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileAxe[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;
            Main.tileFrameImportant[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor); //this seems like a good idea

            ItemDrop = ModContent.ItemType<VerdantStrongVineMaterial>();
            AddMapEntry(new Color(182, 224, 49));
            DustType = DustID.Grass;
            HitSound = SoundID.Grass;
            MinPick = 20;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            bool validAbove = TileHelper.ActiveType(i, j - 1, ModContent.TileType<VerdantGrassLeaves>()) || TileHelper.ActiveType(i, j - 1, Type);
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
            if ((Framing.GetTileSafely(i, j).TileFrameX == 18 && Framing.GetTileSafely(i, j).TileFrameY == 54 || Framing.GetTileSafely(i, j).TileFrameX == 18 && Framing.GetTileSafely(i, j).TileFrameY == 36))
            {
                if (!noItem) Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 16, 16), ModContent.ItemType<RedPetal>());
                Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16 + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), new Vector2(0), Mod.Find<ModGore>("RedPetalFalling").Type, 1);
            }
            if ((Framing.GetTileSafely(i, j).TileFrameX == 36 && Framing.GetTileSafely(i, j).TileFrameY == 36 || Framing.GetTileSafely(i, j).TileFrameX == 36 && Framing.GetTileSafely(i, j).TileFrameY == 54))
            {
                if (!noItem) Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 16, 16), ModContent.ItemType<PinkPetal>());
                Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16 + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), new Vector2(0), Mod.Find<ModGore>("PinkPetalFalling").Type, 1);
            }

            if (Main.tile[i, j + 1].TileType == Type)
                WorldGen.KillTile(i, j + 1, fail, false, false);
        }
    }
}