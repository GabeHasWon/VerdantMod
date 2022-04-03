//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Verdant.Items.Verdant.Materials;

//namespace Verdant.Tiles.Verdant.Basic.Plants
//{
//    internal class VerdantKelp : ModTile
//    {
//        public override void SetDefaults()
//        {
//            Main.tileSolid[Type] = false;
//            Main.tileMergeDirt[Type] = false;
//            Main.tileBlockLight[Type] = false;
//            Main.tileCut[Type] = true;

//            AddMapEntry(new Color(21, 92, 19));
//            dustType = DustID.Grass;
//            soundType = SoundID.Grass;
//        }

//        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

//        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
//        {
//            Tile t = Framing.GetTileSafely(i, j);
//            if (!Framing.GetTileSafely(i, j - 1).active())
//                t.frameX = 18;
//            else
//                t.frameX = 0;

//            if (t.frameX == 0)
//                t.frameY = (short)(Main.rand.Next(6) * 18);
//            else
//            {
//                if (t.frameY >= 108)
//                    t.frameY = (short)((Main.rand.Next(4) * 18) + 108);
//                else
//                    t.frameY = (short)(Main.rand.Next(6) * 18);
//            }
//            return false;
//        }

//        public override void RandomUpdate(int i, int j)
//        {
//            Tile t = Framing.GetTileSafely(i, j);
//            if (!Framing.GetTileSafely(i, j - 1).active() && Main.rand.Next(4) == 0 && t.liquid > 155)
//                WorldGen.PlaceTile(i, j - 1, Type, true, false);

//            if (t.frameX != 0 && Framing.GetTileSafely(i, j).frameY < 108 && Framing.GetTileSafely(i, j).liquid < 155)
//            {
//                if (Main.rand.NextBool(3))
//                    Framing.GetTileSafely(i, j).frameY = (short)((Main.rand.Next(2) * 18) + 54);
                
//            }
//            if (t.frameX == 0)
//            {
//                if (Main.rand.NextBool(1))
//                {
//                    bool[] sides = new bool[2] { Framing.GetTileSafely(i - 1, j).active(), Framing.GetTileSafely(i + 1, j).active() };
//                    int placeSide = -1;
//                    if (!sides[0] && sides[1])
//                        placeSide = 0;
//                    if (sides[0] && !sides[1])
//                        placeSide = 1;
//                    if (!sides[0] && !sides[1])
//                    {
//                        placeSide = Main.rand.Next(2);
//                    }

//                    if (placeSide != -1)
//                    {
//                        t.frameX = 36;
//                        if (placeSide == 0)
//                        {
//                            t.frameY = (short)(18 * Main.rand.Next(3));

//                            WorldGen.PlaceTile(i - 1, j, Type, true, false, -1, 0);
//                            Tile left = Framing.GetTileSafely(i - 1, j);
//                            left.frameX = 54;
//                            left.frameY = (short)((Main.rand.Next(2) * 18) + 108);
//                        }
//                        if (placeSide == 1)
//                        {
//                            t.frameY = (short)((18 * Main.rand.Next(3)) + 54);

//                            WorldGen.PlaceTile(i + 1, j, Type, true, false, -1, 0);
//                            Tile right = Framing.GetTileSafely(i + 1, j);
//                            right.frameX = 54;
//                            right.frameY = (short)((Main.rand.Next(2) * 18) + 144);
//                        }
//                    }
//                }
//            }
//        }

//        public override void NearbyEffects(int i, int j, bool closer)
//        {
//            if (Main.rand.Next(1000) <= 8)
//                Dust.NewDustPerfect(new Vector2(i * 16, j * 16) + new Vector2(2 + Main.rand.Next(12), Main.rand.Next(16)), 34, new Vector2(Main.rand.NextFloat(-0.08f, 0.08f), Main.rand.NextFloat(-0.2f, -0.02f)));

//            if (Framing.GetTileSafely(i, j + 1).liquid < 150 && Framing.GetTileSafely(i, j).liquid < 150)
//                WorldGen.KillTile(i, j, false, false, false);
//        }

//        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
//        {
//            if (Framing.GetTileSafely(i, j).frameY >= 54 && !noItem)
//                Item.NewItem(new Rectangle(i * 16, j * 16, 16, 16), ModContent.ItemType<PinkPetal>());

//            if (Helper.ActiveType(i, j - 1, Type))
//                WorldGen.KillTile(i, j - 1, false, false, false);
//        }

//        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
//        {
//            Tile t = Framing.GetTileSafely(i, j);
//            Texture2D tile = ModContent.GetTexture("Verdant/Tiles/Verdant/Basic/Plants/VerdantKelp");
//            Color col = Lighting.GetColor(i, j);

//            float xOff = (float)Math.Sin((Main.time + (i*24) + (j * 19)) * (0.04f * (!Lighting.NotRetro ? 0f : 1))) * 1.3f;
//            if (Framing.GetTileSafely(i, j + 1).type != Type)
//                xOff *= 0.25f;
//            else if (Framing.GetTileSafely(i, j + 2).type != Type)
//                xOff *= 0.5f;
//            else if (Framing.GetTileSafely(i, j + 3).type != Type)
//                xOff *= 0.75f;
//            spriteBatch.Draw(tile, Helper.TileCustomPosition(i, j) - new Vector2(xOff, 0), new Rectangle(t.frameX, t.frameY, 16, 16), new Color(col.R, col.G, col.B, 255), 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
//            return false;
//        }
//    }
//}