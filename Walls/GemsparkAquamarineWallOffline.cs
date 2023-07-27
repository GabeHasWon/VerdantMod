using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;

namespace Verdant.Walls;

public class GemsparkAquamarineWallOffline : ModWall
{
    public override void SetStaticDefaults()
    {
        Main.wallHouse[Type] = true;

        DustType = DustID.MagnetSphere;
        AddMapEntry(new Color(26, 63, 124));
    }
}