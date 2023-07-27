using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;

namespace Verdant.Walls
{
    public class BubblingWall_Unsafe : ModWall
    {
        public override string Texture => "Verdant/Walls/BubblingWall";

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;

            DustType = DustID.UnusedWhiteBluePurple;
            AddMapEntry(new Color(183, 164, 143));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.rand.NextBool(40))
                Dust.NewDust(new Vector2(i, j) * 16, 16, 16, DustID.BreatheBubble, 0, 0, 0, default, Main.rand.NextFloat(1.4f, 1.8f));
        }
    }

    public class BubblingWall : BubblingWall_Unsafe
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;

            DustType = DustID.UnusedWhiteBluePurple;
            AddMapEntry(new Color(183, 164, 143));
        }
    }
}