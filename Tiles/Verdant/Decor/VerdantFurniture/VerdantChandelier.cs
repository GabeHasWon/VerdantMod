using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture
{
    internal class VerdantChandelier : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom, 2, 0);
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.Origin = new Point16(0, 2);
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;

            QuickTile.SetMulti(this, 2, 3, DustID.Grass, SoundID.Grass, false, new Color(20, 82, 39), false, false, false, "");

            Main.tileFrameImportant[Type] = true;
            Main.tileCut[Type] = false;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 2 : 5;

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Framing.GetTileSafely(i, j).frameX == 0 && Framing.GetTileSafely(i, j).frameY == 18)
            {
                Vector2 p = (new Vector2(i, j) * 16);
                Lighting.AddLight(p + new Vector2(16, 16), new Vector3(0.1f, 0.03f, 0.06f) * 14);
                Lighting.AddLight(p + new Vector2(10, 30), new Vector3(0.1f, 0.03f, 0.06f) * 14);
                Lighting.AddLight(p + new Vector2(26, 26), new Vector3(0.1f, 0.03f, 0.06f) * 14);
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new Vector2(i * 16, j * 16), ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantChandelierBlock>(), 1);
            for (int v = 0; v < 4; ++v)
            {
                Vector2 off = new Vector2(Main.rand.Next(32), Main.rand.Next(54));
                Gore.NewGore(new Vector2(i, j) * 16 + off, new Vector2(0), Main.rand.NextBool(2) ? mod.GetGoreSlot("Gores/Verdant/LushLeaf") : mod.GetGoreSlot("Gores/Verdant/RedPetalFalling"), 1);
            }
        }
    }
}