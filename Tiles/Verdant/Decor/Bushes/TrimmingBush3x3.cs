using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Bushes;

namespace Verdant.Tiles.Verdant.Decor.Bushes;

public class TrimmingBush3x3 : ModTile, IBush
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(33, 124, 22));
        RegisterItemDrop(ModContent.ItemType<Bush3x3Item>());
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    bool IBush.CanBeTrimmed(int x, int y) => true;

    void IBush.ChooseTrim(int x, int y)
    {
        Tile tile = Main.tile[x, y];
        int i = x - (tile.TileFrameX / 18 % 3);
        int j = y - (tile.TileFrameY / 18);
        int frameOffset = Main.rand.Next(1, 3);

        for (int rX = i; rX < i + 3; ++rX)
        {
            for (int rY = j; rY < j + 3; ++rY)
            {
                if (Main.tile[rX, rY].TileFrameX > 36)
                    Main.tile[rX, rY].TileFrameX = (short)(Main.tile[rX, rY].TileFrameX % 54);

                Main.tile[rX, rY].TileFrameX += (short)(frameOffset * 54);
                int repeats = Main.rand.Next(2, 5);

                for (int k = 0; k < repeats; ++k)
                {
                    Vector2 dir = new Vector2(i, j).ToWorldCoordinates(16, 16);
                    dir = Vector2.Normalize(new Vector2(rX, rY).ToWorldCoordinates() - dir) * Main.rand.NextFloat(0.15f, 1f);
                    Dust.NewDust(new Vector2(rX, rY).ToWorldCoordinates(0, 0), 16, 16, DustID.GrassBlades, dir.X, dir.Y);
                }

                SoundEngine.PlaySound(SoundID.Grass, new Vector2(i, j).ToWorldCoordinates(16, 16));
            }
        }
    }
}