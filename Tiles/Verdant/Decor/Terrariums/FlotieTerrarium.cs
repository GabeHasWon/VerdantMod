using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.NPCs.Passive.Floties;

namespace Verdant.Tiles.Verdant.Decor.Terrariums;

public class FlotieTerrarium : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;
        Main.tileSolidTop[Type] = true;
        Main.tileTable[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Width = 4;
        TileObjectData.newTile.Height = 5;
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16 };
        TileObjectData.newTile.Origin = new Point16(2, 3);
        TileObjectData.addTile(Type);

        DustType = DustID.Glass;

        AddMapEntry(new Color(22, 51, 81));
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];

        if (tile.TileFrameX == 18 && tile.TileFrameY == 36)
        {
            Main.instance.LoadNPC(ModContent.NPCType<Flotie>());
            Texture2D tex = TextureAssets.Npc[ModContent.NPCType<Flotie>()].Value;
            float sineOffset = (i * MathHelper.PiOver4) + (j * MathHelper.PiOver4 / 2f) + Main.GameUpdateCount;
            float x = MathF.Floor(MathF.Sin(sineOffset * 0.0175f) * 4);
            Vector2 off = new(2 + x, MathF.Floor(MathF.Sin(sineOffset * 0.03f) * 10 + 18));

            spriteBatch.Draw(tex, TileHelper.TileCustomPosition(i, j, off), new Rectangle(0, x > 0 ? 0 : 50, 38, 48), Color.Lerp(Lighting.GetColor(i, j), Color.LightPink, 0.4f));

            Texture2D glowTex = Flotie.glowTexture.Value;
            spriteBatch.Draw(glowTex, TileHelper.TileCustomPosition(i, j, off), new Rectangle(0, x > 0 ? 0 : 50, 38, 48), Color.White);
        }
        return true;
    }
}