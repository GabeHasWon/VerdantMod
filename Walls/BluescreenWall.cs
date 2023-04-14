using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Verdant.Walls;

public class BluescreenWall : ModWall
{
    public override void SetStaticDefaults()
    {
        Main.wallHouse[Type] = true;
        AddMapEntry(Color.Blue);
    }
}