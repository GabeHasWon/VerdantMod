using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;

namespace Verdant.Walls;

public class PuffWall : ModWall
{
    public override void SetStaticDefaults()
    {
        Main.wallHouse[Type] = true;
        DustType = DustID.t_BorealWood;
        ItemDrop = ModContent.ItemType<PuffWallItem>();
        AddMapEntry(Color.Blue);
    }
}