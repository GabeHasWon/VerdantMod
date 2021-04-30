using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Walls.Verdant
{
    public class VerdantRedPetalWall_Unsafe : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            dustType = DustID.Grass;
            drop = 0;
            AddMapEntry(new Color(81, 4, 22));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }

    public class VerdantRedPetalWall : ModWall
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "Verdant/Walls/Verdant/VerdantRedPetalWall_Unsafe";
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            dustType = DustID.Grass;
            drop = ModContent.ItemType<Items.Verdant.Blocks.Walls.VerdantRedPetalWallItem>();
            AddMapEntry(new Color(81, 4, 22));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}