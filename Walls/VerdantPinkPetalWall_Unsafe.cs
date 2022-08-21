using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Walls
{
    public class VerdantPinkPetalWall_Unsafe : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            DustType = DustID.Grass;
            ItemDrop = 0;
            AddMapEntry(new Color(112, 0, 59));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }

    public class VerdantPinkPetalWall : ModWall
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            texture = "Verdant/Walls/VerdantPinkPetalWall_Unsafe";
            return base.IsLoadingEnabled(ref name, ref texture);
        }

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            DustType = DustID.Grass;
            ItemDrop = ModContent.ItemType<Items.Verdant.Blocks.Walls.VerdantPinkPetalWallItem>();
            AddMapEntry(new Color(112, 0, 59));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}