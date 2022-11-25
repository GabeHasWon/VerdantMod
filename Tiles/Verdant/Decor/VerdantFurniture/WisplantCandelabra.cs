using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Systems;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture
{
    internal class WisplantCandelabra : ModTile
    {
        public override void SetStaticDefaults()
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

        public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 32, ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantCandelabraItem>());

        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int topY = j - tile.TileFrameY / 18 % 3;
            short frameAdjustment = (short)(tile.TileFrameX >= 18 ? 18 : -18);
            for (int k = 0; k < 2; ++k)
            {
                for (int b = 0; b < 2; ++b)
                {
                    Main.tile[i + k, topY + b].TileFrameX += frameAdjustment;
                    Wiring.SkipWire(i + k, topY + b);
                }
            }
            NetMessage.SendTileSquare(-1, i, topY + 1, 1, TileChangeType.None);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Vector3 light = new Vector3(0.5f, 0.16f, 0.30f) * 3f;
            if (Framing.GetTileSafely(i, j).TileFrameX == 0)
            {
                r = light.X;
                g = light.Y;
                b = light.Z;
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Main.gamePaused)
                return;

            RandomUpdating.CircularUpdate(i, j, 10, 1600, (i, j) =>
            {
                Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.TerraBlade, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            });
        }

        public override void RandomUpdate(int i, int j)
        {
            RandomUpdating.CircularUpdate(i, j, 10, 80, (i, j) =>
            {
                Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.TerraBlade, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            });
        }
    }
}
