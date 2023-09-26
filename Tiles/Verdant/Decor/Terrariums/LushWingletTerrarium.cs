using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using Verdant.NPCs.Passive.Fish;
using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ObjectData;
using Verdant.NPCs.Passive;

namespace Verdant.Tiles.Verdant.Decor.Terrariums;

public class LushWingletTerrarium : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;
        Main.tileSolidTop[Type] = true;
        Main.tileTable[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Width = 5;
        TileObjectData.newTile.Height = 5;
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16 };
        TileObjectData.newTile.Origin = new Point16(3, 3);
        TileObjectData.addTile(Type);

        DustType = DustID.Glass;

        AddMapEntry(new Color(22, 51, 81));
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];
        if (tile.TileFrameX == 36 && tile.TileFrameY == 36)
        {
            // lol good luck reading this
            Vector2[] offsets =   new[] { new Vector2(0), new(-5, 5), new(-7, 7), new(-15, 10), new(-15, 10), new(-15, 10), new(0, 8), new(10, 6), new(10, 5), new(10, 5), new(10, 5), new(0, -5), new(-10, -15), new(-15, -20), new(-15, -20), new(-15, -20), new(-7.5f, -10), new(0), new(0)  };
            float[] rotations = new[] { -1f, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1 };

            Main.instance.LoadNPC(ModContent.NPCType<SmallFly>());
            Texture2D tex = TextureAssets.Npc[ModContent.NPCType<SmallFly>()].Value;
            float offset = (Main.GameUpdateCount * 0.06f) + ((i + j) * MathHelper.PiOver2);
            int index = (int)Math.Ceiling(offset) % offsets.Length;
            Vector2 off = Vector2.Lerp(offsets[index], offsets[index == offsets.Length - 1 ? 0 : index + 1], offset % 1);
            SpriteEffects effect = rotations[index] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            var src = new Rectangle(0, ((int)((offset * 5) % 2) * 20), 26, 18);

            spriteBatch.Draw(tex, TileHelper.TileCustomPosition(i, j, new Vector2(MathF.Round(off.X), off.Y).Floor()), src, Lighting.GetColor(i, j), 0f, new(11, 12), 1f, effect, 0);
        }
    }
}