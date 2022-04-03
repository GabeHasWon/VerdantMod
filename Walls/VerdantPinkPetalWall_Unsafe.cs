using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Walls
{
    public class VerdantPinkPetalWall_Unsafe : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            dustType = DustID.Grass;
            drop = 0;
            AddMapEntry(new Color(112, 0, 59));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }

    public class VerdantPinkPetalWall : ModWall
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "Verdant/Walls/Verdant/VerdantPinkPetalWall_Unsafe";
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            dustType = DustID.Grass;
            drop = ModContent.ItemType<Items.Verdant.Blocks.Walls.VerdantPinkPetalWallItem>();
            AddMapEntry(new Color(112, 0, 59));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}