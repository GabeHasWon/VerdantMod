using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Tiles.Verdant.Basic.Blocks
{
    internal class VerdantGrassLeaves : ModTile
    {
        public override void SetStaticDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.GrassBlades, SoundID.Grass, new Color(44, 160, 54), ModContent.ItemType<LushLeaf>(), "", true, false);
            QuickTile.MergeWith(Type, ModContent.TileType<LushSoil>(), ModContent.TileType<VerdantPinkPetal>(), ModContent.TileType<VerdantRedPetal>(), TileID.LivingWood, TileID.Stone, TileID.Dirt, TileID.Grass);

            Main.tileBrick[Type] = true;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if ((!Framing.GetTileSafely(i, j + 1).HasTile || !Main.tileSolid[Framing.GetTileSafely(i, j + 1).TileType]) && Main.rand.NextBool(1945))
                Gore.NewGorePerfect(new EntitySource_TileUpdate(i, j), (new Vector2(i, j + 1) * 16) - new Vector2(0, 2), new Vector2(0, 0), Mod.Find<ModGore>("VerdantDroplet").Type, 1f);
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile self = Framing.GetTileSafely(i, j);
            //vine
            if (TileHelper.ValidBottom(self) && !Framing.GetTileSafely(i, j + 1).HasTile && Main.rand.NextBool(3))
            {
                WorldGen.PlaceTile(i, j + 1, ModContent.TileType<VerdantVine>(), true, false);
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
            }
            //decor 1x1
            if (TileHelper.ValidTop(self) && !Framing.GetTileSafely(i, j - 1).HasTile && Main.rand.NextBool(5))
            {
                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<VerdantDecor1x1>(), true, false, -1, Main.rand.Next(7));
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
            //decor 2x1
            if (TileHelper.ValidTop(self) && TileHelper.ValidTop(i + 1, j) && !Framing.GetTileSafely(i, j - 1).HasTile && !Framing.GetTileSafely(i + 1, j - 1).HasTile && Main.rand.NextBool(8))
            {
                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<VerdantDecor2x1>(), true, false, -1, Main.rand.Next(6));
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
            //lily
            if (TileHelper.ValidTop(self) && !Framing.GetTileSafely(i, j - 1).HasTile && Framing.GetTileSafely(i, j - 1).LiquidAmount > 150 && Main.rand.NextBool(3))
            {
                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<VerdantLillie>(), true, false, -1, 0);
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
            //bouncebloom
            if (NPC.downedBoss1 && Main.rand.NextBool(110) && TileHelper.ValidTop(self) && TileHelper.ValidTop(i - 1, j) && TileHelper.ValidTop(i + 1, j) && Helper.AreaClear(i - 1, j - 2, 3, 2))
            {
                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<Bouncebloom>(), true, false, -1, 0);
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
            //lightbulb
            if (Main.rand.NextBool(220) && TileHelper.ValidTop(self) && TileHelper.ValidTop(i + 1, j) && Helper.AreaClear(i, j - 2, 2, 2))
            {
                WorldGen.PlaceTile(i, j - 2, ModContent.TileType<VerdantLightbulb>(), true, false, -1, Main.rand.Next(3));
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
            //decor 2x2
            if (Main.rand.NextBool(15) && TileHelper.ValidTop(self) && TileHelper.ValidTop(i + 1, j) && Helper.AreaClear(i, j - 2, 2, 2))
            {
                WorldGen.PlaceTile(i, j - 2, ModContent.TileType<VerdantDecor2x2>(), true, false, -1, Main.rand.Next(8));
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
            //decor 1x2
            if (Main.rand.NextBool(7) && TileHelper.ValidTop(self) && Helper.AreaClear(i, j - 2, 1, 2))
            {
                WorldGen.PlaceTile(i, j - 2, ModContent.TileType<VerdantDecor1x2>(), true, false, -1, Main.rand.Next(6));
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
            //decor 1x3
            if (Main.rand.NextBool(8) && TileHelper.ValidTop(self) && Helper.AreaClear(i, j - 3, 1, 3))
            {
                WorldGen.PlaceTile(i, j - 3, ModContent.TileType<VerdantDecor1x3>(), true, false, -1, Main.rand.Next(7));
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
        }
    }
}