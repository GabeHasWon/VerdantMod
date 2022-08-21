using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;

namespace Verdant.Walls
{
    public class LushSoilWall_Unsafe : ModWall
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

    public class LushSoilWall : ModWall
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            texture = "Verdant/Walls/LushSoilWall_Unsafe";
            return base.IsLoadingEnabled(ref name, ref texture);
        }

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            DustType = DustID.Grass;
            ItemDrop = ModContent.ItemType<LushSoilWallItem>();
            AddMapEntry(new Color(50, 15, 15));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}