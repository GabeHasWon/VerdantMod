using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;

namespace Verdant.Walls
{
    public class BackslateWall_Unsafe : ModWall
    {
        public override string Texture => "Verdant/Walls/BackslateWall";

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;

            DustType = DustID.UnusedWhiteBluePurple;
            AddMapEntry(new Color(183, 164, 143));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }

    public class BackslateWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;

            DustType = DustID.UnusedWhiteBluePurple;
            AddMapEntry(new Color(183, 164, 143));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}