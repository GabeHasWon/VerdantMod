using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Plants;

internal class VineRopeTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = false;
        Main.tileCut[Type] = false;
        Main.tileMergeDirt[Type] = false;
        Main.tileBlockLight[Type] = false;
        Main.tileRope[Type] = true;
        Main.tileFrameImportant[Type] = false;

        ItemDrop = ModContent.ItemType<Items.Verdant.Blocks.VineRopeItem>();

        AddMapEntry(new Color(23, 102, 32));

        DustType = DustID.Grass;
        HitSound = SoundID.Grass;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

    //public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
    //{
    //    bool above = WorldGen.SolidTile(i, j - 1) || TileHelper.ActiveType(i, j - 1, Type);
    //    bool below = WorldGen.SolidTile(i, j + 1) || TileHelper.ActiveType(i, j + 1, Type);

    //    int frameX = 0, frameY = 0;

    //    if (above && below)
    //    {
    //        frameX = 0;
    //        frameY = Main.rand.Next(4);
    //    }
    //    else if (above && !below)
    //    {
    //        frameX = 1;
    //        frameY = Main.rand.Next(2);
    //    }
    //    else if (!above && below)
    //    {
    //        frameX = 1;
    //        frameY = Main.rand.Next(2) + 2;
    //    }
    //    else if (!above && !below)
    //    {
    //        frameX = Main.rand.Next(2);
    //        frameY = 4;
    //    }

    //    Tile tile = Main.tile[i, j];
    //    tile.TileFrameX = (short)(frameX * 18);
    //    tile.TileFrameY = (short)(frameY * 18);
    //    return false;
    //}
}