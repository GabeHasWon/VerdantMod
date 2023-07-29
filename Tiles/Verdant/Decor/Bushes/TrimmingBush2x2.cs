using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Bushes;

namespace Verdant.Tiles.Verdant.Decor.Bushes;

public class TrimmingBush2x2 : ModTile, IBush
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(33, 124, 22));
        RegisterItemDrop(ModContent.ItemType<Bush2x2Item>());
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    bool IBush.CanBeTrimmed(int x, int y) => true;

    void IBush.ChooseTrim(int x, int y)
    {
        Tile tile = Main.tile[x, y];
        int i = x - (tile.TileFrameX / 18 % 2);
        int j = y - (tile.TileFrameY / 18);
        int frameOffset = Main.rand.Next(1, 5);

        for (int rX = i; rX < i + 2; ++rX)
        {
            for (int rY = j; rY < j + 2; ++rY)
            {
                if (Main.tile[rX, rY].TileFrameX > 18)
                    Main.tile[rX, rY].TileFrameX = (short)(Main.tile[rX, rY].TileFrameX % 36);

                Main.tile[rX, rY].TileFrameX += (short)(frameOffset * 36);
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