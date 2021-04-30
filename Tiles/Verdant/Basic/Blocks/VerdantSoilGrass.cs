using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Plants;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Tiles.Verdant.Basic.Blocks
{
    internal class VerdantSoilGrass : ModTile
    {
        public override void SetDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.GrassBlades, SoundID.Grass, new Color(44, 160, 54), ItemType<LushLeaf>(), "", true, false);
            QuickTile.MergeWith(Type, TileType<LushSoil>(), TileType<VerdantPinkPetal>(), TileType<VerdantRedPetal>(), TileID.LivingWood, TileID.Stone, TileID.Dirt, TileID.Grass);
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile self = Framing.GetTileSafely(i, j);
            //vine
            if (TileHelper.ValidBottom(self) && !Framing.GetTileSafely(i, j + 1).active() && Main.rand.Next(3) == 0)
            {
                WorldGen.PlaceTile(i, j + 1, TileType<VerdantVine>(), true, false);
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
            }
            //decor 1x1
            if (TileHelper.ValidTop(self) && !Framing.GetTileSafely(i, j - 1).active() && Main.rand.Next(5) == 0)
            {
                WorldGen.PlaceTile(i, j - 1, TileType<VerdantDecor1x1>(), true, false, -1, Main.rand.Next(7));
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
            //decor 2x1
            if (TileHelper.ValidTop(self) && TileHelper.ValidTop(i + 1, j) && !Framing.GetTileSafely(i, j - 1).active() && !Framing.GetTileSafely(i + 1, j - 1).active() && Main.rand.Next(8) == 0)
            {
                WorldGen.PlaceTile(i, j - 1, TileType<VerdantDecor2x1>(), true, false, -1, Main.rand.Next(4));
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
            //lily
            if (TileHelper.ValidTop(self) && !Framing.GetTileSafely(i, j - 1).active() && Framing.GetTileSafely(i, j - 1).liquid > 150 && Main.rand.Next(3) == 0)
            {
                WorldGen.PlaceTile(i, j - 1, TileType<VerdantLillie>(), true, false, -1, 0);
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
            //bouncebloom
            if (NPC.downedBoss1 && Main.rand.Next(110) == 0 && TileHelper.ValidTop(self) && TileHelper.ValidTop(i - 1, j) && TileHelper.ValidTop(i + 1, j) && Helper.AreaClear(i - 1, j -2, 3, 2))
            {
                WorldGen.PlaceTile(i, j - 1, TileType<Bouncebloom>(), true, false, -1, 0);
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
            //lightbulb
            if (Main.rand.Next(220) == 0 && TileHelper.ValidTop(self) && TileHelper.ValidTop(i + 1, j) && Helper.AreaClear(i, j - 2, 2, 2))
            {
                WorldGen.PlaceTile(i, j - 2, TileType<VerdantLightbulb>(), true, false, -1, 0);
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
            //decor 2x2
            if (Main.rand.Next(15) == 0 && TileHelper.ValidTop(self) && TileHelper.ValidTop(i + 1, j) && Helper.AreaClear(i, j - 2, 2, 2))
            {
                WorldGen.PlaceTile(i, j - 2, TileType<VerdantDecor2x2>(), true, false, -1, 0);
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
            //decor 1x2
            if (Main.rand.Next(11) == 0 && TileHelper.ValidTop(self) && Helper.AreaClear(i, j - 2, 1, 2))
            {
                WorldGen.PlaceTile(i, j - 2, TileType<VerdantDecor1x2>(), true, false, -1, 0);
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return TileType<LushSapling>();
        }
    }
}