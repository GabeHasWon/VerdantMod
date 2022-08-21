using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;

namespace Verdant.Walls
{
    public class LivingLushWoodWall_Unsafe : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            DustType = DustID.Grass;
            ItemDrop = ItemID.None;
            AddMapEntry(new Color(50, 15, 15));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }

    public class LivingLushWoodWall : ModWall
    {
        public override string Texture => "Verdant/Walls/LivingLushWoodWall_Unsafe";

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            DustType = DustID.Grass;
            ItemDrop = ModContent.ItemType<LivingLushWoodWallItem>();
            AddMapEntry(new Color(50, 15, 15));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}