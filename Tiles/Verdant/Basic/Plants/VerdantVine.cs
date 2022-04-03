using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Plants
{
    internal class VerdantVine : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileCut[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;
            drop = 0;
            AddMapEntry(new Color(24, 135, 28));
            dustType = DustID.Grass;
            soundType = SoundID.Grass;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

        public override void RandomUpdate(int i, int j)
        {
            if (!Main.tile[i, j + 1].active() && Main.rand.Next(10) == 0)
                WorldGen.PlaceTile(i, j + 1, Type, true, false);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Main.tile[i, j + 1].type == Type)
                WorldGen.KillTile(i, j + 1, false, false, true);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (!Main.tile[i, j - 1].active())
                WorldGen.KillTile(i, j);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile t = Framing.GetTileSafely(i, j);

            float sine = (float)Math.Sin((i + j) * MathHelper.ToRadians(20) + Main.GameUpdateCount * 0.02f) * 1f;
            spriteBatch.Draw(Main.tileTexture[Type], TileHelper.TileCustomPosition(i, j, new Vector2(sine, 0)), new Rectangle(t.frameX, t.frameY, 16, 16), Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}