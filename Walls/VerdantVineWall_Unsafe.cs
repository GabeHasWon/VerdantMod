using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Walls
{
    public class VerdantVineWall_Unsafe : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            dustType = DustID.Grass;
            drop = 0;
            AddMapEntry(new Color(29, 76, 8));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}