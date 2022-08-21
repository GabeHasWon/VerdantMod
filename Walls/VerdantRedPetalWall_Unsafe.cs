using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Walls
{
    public class VerdantRedPetalWall_Unsafe : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            DustType = DustID.Grass;
            ItemDrop = 0;
            AddMapEntry(new Color(81, 4, 22));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }

    public class VerdantRedPetalWall : ModWall
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            texture = "Verdant/Walls/VerdantRedPetalWall_Unsafe";
            return base.IsLoadingEnabled(ref name, ref texture);
        }

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            DustType = DustID.Grass;
            ItemDrop = ModContent.ItemType<Items.Verdant.Blocks.Walls.VerdantRedPetalWallItem>();
            AddMapEntry(new Color(81, 4, 22));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}