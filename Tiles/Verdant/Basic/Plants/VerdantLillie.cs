using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Basic.Plants
{
    internal class VerdantLillie : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;

            AddMapEntry(new Color(21, 92, 19));
            dustType = DustID.Grass;
            soundType = SoundID.Grass;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            if (!Framing.GetTileSafely(i, j - 1).active())
                Framing.GetTileSafely(i, j).frameX = 18;
            else
                Framing.GetTileSafely(i, j).frameX = 0;

            if (Framing.GetTileSafely(i, j).frameX == 0)
                Framing.GetTileSafely(i, j).frameY = (short)(Main.rand.Next(3) * 18);
            else
            {
                if (Framing.GetTileSafely(i, j).frameY >= 54)
                    Framing.GetTileSafely(i, j).frameY = (short)((Main.rand.Next(2) * 18) + 54);
                else
                    Framing.GetTileSafely(i, j).frameY = (short)(Main.rand.Next(3) * 18);
            }
            return false;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (!Framing.GetTileSafely(i, j - 1).active() && Main.rand.Next(2) == 0 && Framing.GetTileSafely(i, j).liquid > 155)
                WorldGen.PlaceTile(i, j - 1, Type, true, false);

            if (Framing.GetTileSafely(i, j).frameX != 0 && Framing.GetTileSafely(i, j).frameY < 54 && Main.rand.Next(1) == 0 && Framing.GetTileSafely(i, j).liquid < 155)
                Framing.GetTileSafely(i, j).frameY = (short)((Main.rand.Next(2) * 18) + 54);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Main.rand.Next(1000) <= 8)
                Dust.NewDustPerfect(new Vector2(i * 16, j * 16) + new Vector2(2 + Main.rand.Next(12), Main.rand.Next(16)), 34, new Vector2(Main.rand.NextFloat(-0.08f, 0.08f), Main.rand.NextFloat(-0.2f, -0.02f)));

            if (Framing.GetTileSafely(i, j + 1).liquid < 150 && Framing.GetTileSafely(i, j).liquid < 150)
                WorldGen.KillTile(i, j, false, false, false);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Framing.GetTileSafely(i, j).frameY >= 54 && !noItem)
                Item.NewItem(new Rectangle(i * 16, j * 16, 16, 16), ModContent.ItemType<PinkPetal>());

            if (TileHelper.ActiveType(i, j - 1, Type))
                WorldGen.KillTile(i, j - 1, false, false, false);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile t = Framing.GetTileSafely(i, j);
            Texture2D tile = ModContent.GetTexture("Verdant/Tiles/Verdant/Basic/Plants/VerdantLillie");
            Color col = Lighting.GetColor(i, j);

            float xOff = (float)Math.Sin((Main.GameUpdateCount + (i*24) + (j * 19)) * (0.04f * (!Lighting.NotRetro ? 0f : 1))) * 1.3f;
            if (Framing.GetTileSafely(i, j + 1).type != Type)
                xOff *= 0.25f;
            else if (Framing.GetTileSafely(i, j + 2).type != Type)
                xOff *= 0.5f;
            else if (Framing.GetTileSafely(i, j + 3).type != Type)
                xOff *= 0.75f;
            spriteBatch.Draw(tile, TileHelper.TileCustomPosition(i, j) - new Vector2(xOff, 0), new Rectangle(t.frameX, t.frameY, 16, 16), new Color(col.R, col.G, col.B, 255), 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}