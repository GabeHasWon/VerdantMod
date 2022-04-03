using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Walls
{
    public class VerdantLeafWall_Unsafe : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            dustType = DustID.Grass;
            drop = 0;
            AddMapEntry(new Color(20, 82, 39));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        //public override void RandomUpdate(int i, int j)
        //{
        //    if (Main.rand.Next(160) == 0 && WorldGen.TileEmpty(i, j) && WorldGen.TileEmpty(i + 1, j) && Framing.GetTileSafely(i + 1, j).wall == Type && WorldGen.TileEmpty(i + 1, j + 1) && Framing.GetTileSafely(i + 1, j + 1).wall == Type && WorldGen.TileEmpty(i, j + 1) && Framing.GetTileSafely(i, j + 1).wall == Type)
        //        GenHelper.PlaceMultitile(new Point(i, j), ModContent.TileType<Tiles.Verdant.Mounted.Flower_2x2>(), Main.rand.Next(4));
        //    if (Main.rand.Next(300) == 0 && WorldGen.TileEmpty(i, j) && WorldGen.TileEmpty(i + 1, j) && Framing.GetTileSafely(i + 1, j).wall == Type && WorldGen.TileEmpty(i + 1, j + 1) && Framing.GetTileSafely(i + 1, j + 1).wall == Type && WorldGen.TileEmpty(i, j + 1) && Framing.GetTileSafely(i, j + 1).wall == Type)
        //        GenHelper.PlaceMultitile(new Point(i, j), ModContent.TileType<Tiles.Verdant.Mounted.MountedLightbulb_2x2>(), Main.rand.Next(4));
        //}
    }
}