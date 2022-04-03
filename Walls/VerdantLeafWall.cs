using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;

namespace Verdant.Walls
{
    public class VerdantLeafWall : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = true;
            dustType = DustID.Grass;
            drop = ModContent.ItemType<VerdantLeafWallItem>();
            AddMapEntry(new Color(20, 82, 39));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}