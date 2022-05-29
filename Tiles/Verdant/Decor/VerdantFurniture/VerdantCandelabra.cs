using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture
{
    internal class VerdantCandelabra : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.WaterDeath = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

            AddMapEntry(new Color(253, 221, 3), CreateMapEntryName());
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 16, 32, ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantCandelabraItem>());

        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int topY = j - tile.frameY / 18 % 3;
            short frameAdjustment = (short)(tile.frameX >= 18 ? 18 : -18);
            for (int k = 0; k < 2; ++k)
            {
                for (int b = 0; b < 2; ++b)
                {
                    Main.tile[i + k, topY + b].frameX += frameAdjustment;
                    Wiring.SkipWire(i + k, topY + b);
                }
            }
            NetMessage.SendTileSquare(-1, i, topY + 1, 1, TileChangeType.None);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            if (tile.frameX <= 18 && tile.frameY == 0)
            {
                r = 0.3f;
                g = 0.09f;
                b = 0.18f;
            }
        }
    }
}
