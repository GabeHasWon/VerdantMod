using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;

namespace Verdant.Walls;

public class GemsparkAquamarineWall : ModWall
{
    public override void SetStaticDefaults()
    {
        Main.wallHouse[Type] = true;
        Main.wallLight[Type] = true;

        DustType = DustID.MagnetSphere;
        ItemDrop = ModContent.ItemType<BackslateBubblingWallItem>();
        AddMapEntry(new Color(26, 94, 143));
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) => (r, g, b) = (0.498f, 1, 0.831f);
}