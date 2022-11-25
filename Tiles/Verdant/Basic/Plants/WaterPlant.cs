using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;

namespace Verdant.Tiles.Verdant.Basic.Plants
{
    internal class WaterPlant : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Origin = new Point16(1, 0);
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidBottom | AnchorType.SolidTile, 1, 1);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.addTile(Type);

            TileID.Sets.CountsAsWaterSource[Type] = true;

            AddMapEntry(new Color(76, 198, 255));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 2 : 5;

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16, j * 16), ModContent.ItemType<Items.Verdant.Blocks.Plants.WaterPlantItem>(), 1);

            if (Main.netMode != NetmodeID.Server)
            {
                for (int v = 0; v < 4; ++v)
                {
                    Vector2 off = new Vector2(Main.rand.Next(54), Main.rand.Next(32));
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16 + off, new Vector2(0), Mod.Find<ModGore>("PinkPetalFalling").Type, 1);
                }
            }
        }
    }
}