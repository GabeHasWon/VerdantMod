using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Trees
{
    internal class VerdantTree : ModTile
    {
        public override void SetDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.t_BorealWood, SoundID.Dig, new Color(142, 62, 32), ModContent.ItemType<VerdantWoodBlock>(), "Tree", false, false, false, false);
            Main.tileFrameImportant[Type] = true;
            Main.tileAxe[Type] = true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = (fail ? 1 : 3);

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) => false;

        /// <summary>Spawns a Lush Wood tree or adjacent type.</summary>
        /// <param name="i">X coordinate in-world.</param>
        /// <param name="j">Y coordinate in-world.</param>
        /// <param name="type">Defaults to this mod's Lush Tree.</param>
        /// <param name="r">Use WorldGen.genRand for world generation; Main.rand otherwise.</param>
        /// <param name="minSize">Minimum height.</param>
        /// <param name="maxSize">Maximum height.</param>
        /// <param name="leaves">If true, spawn leaf gores.</param>
        /// <param name="leavesType">Type of leaf gores spawned, if spawned.</param>
        /// <returns></returns>
        public static bool Spawn(int i, int j, int type = -1, UnifiedRandom r = null, int minSize = 5, int maxSize = 18, bool leaves = false, int leavesType = -1, bool saplingExists = false)
        {
            if (type == -1) type = ModContent.TileType<VerdantTree>(); //Sets default types
            if (leavesType == -1) leavesType = GoreID.TreeLeaf_Jungle;

            if (r == null) //For use in worldgen instead of Main.rand
                r = Main.rand;

            if (saplingExists)
            {
                WorldGen.KillTile(i, j, false, false, true);
                WorldGen.KillTile(i, j - 1, false, false, true);
            }

            int height = r.Next(minSize, maxSize); //Height & trunk
            for (int k = 1; k < height; ++k)
            {
                if (Helper.SolidTile(i, j - k))
                {
                    height = k - 2;
                    break;
                }
            }

            if (height < 4 || height < minSize) return false;

            bool[] extraPlaces = new bool[5];
            for (int k = -2; k < 3; ++k) //Checks base
            {
                extraPlaces[k + 2] = false;
                if ((Helper.SolidTopTile(i + k, j + 1) || Helper.SolidTile(i + k, j + 1)) && !Framing.GetTileSafely(i + k, j).active())
                    extraPlaces[k + 2] = true;
            }

            if (!extraPlaces[1]) extraPlaces[0] = false;
            if (!extraPlaces[3]) extraPlaces[4] = false;

            if (!extraPlaces[2]) return false;

            extraPlaces = new bool[5] { false, false, true, false, false };

            for (int k = -2; k < 3; ++k) //Places base
            {
                if (extraPlaces[k + 2])
                    WorldGen.PlaceTile(i + k, j, type, true);
                else
                    continue;

                Framing.GetTileSafely(i + k, j).frameX = (short)((k + 2) * 18);
                Framing.GetTileSafely(i + k, j).frameY = (short)(r.Next(3) * 18);

                if (!extraPlaces[0] && k == -1)
                    Framing.GetTileSafely(i + k, j).frameX = 216;
                if (!extraPlaces[3] && k == 1)
                    Framing.GetTileSafely(i + k, j).frameX = 234;

                if (!extraPlaces[1] && !extraPlaces[3] && k == 0) Framing.GetTileSafely(i + k, j).frameX = 90;
                if (extraPlaces[1] && !extraPlaces[3] && k == 0) Framing.GetTileSafely(i + k, j).frameX = 252;
                if (!extraPlaces[1] && extraPlaces[3] && k == 0) Framing.GetTileSafely(i + k, j).frameX = 270;
            }

            for (int k = 1; k < height; ++k)
            {
                WorldGen.PlaceTile(i, j - k, type, true);
                Framing.GetTileSafely(i, j - k).frameX = 90;
                Framing.GetTileSafely(i, j - k).frameY = (short)(r.Next(3) * 18);

                if (k == height - 1)
                {
                    if (r.Next(12) == 0) Framing.GetTileSafely(i, j - k).frameX = 180;
                    else Framing.GetTileSafely(i, j - k).frameX = 198;
                }
                else if (r.Next(4) == 0)
                {
                    int side = r.Next(2);
                    if (side == 0 && !Framing.GetTileSafely(i - 1, j - k).active())
                    {
                        WorldGen.PlaceTile(i - 1, j - k, type, true);
                        Framing.GetTileSafely(i, j - k).frameX = 162;
                        Framing.GetTileSafely(i - 1, j - k).frameX = 108;
                    }
                    else if (side == 1 && !Framing.GetTileSafely(i + 1, j - k).active())
                    {
                        WorldGen.PlaceTile(i + 1, j - k, type, true);
                        Framing.GetTileSafely(i, j - k).frameX = 144;
                        Framing.GetTileSafely(i + 1, j - k).frameX = 126;
                    }
                }

                if (leaves)
                {
                    if (r.Next(4) <= 1)
                    {
                        int rnd = r.Next(2, 5);
                        for (int l = 0; l < rnd; ++l)
                            Gore.NewGore((new Vector2(i, j - k) * 16) + new Vector2(8 + r.Next(-4, 5), 8), new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), leavesType);
                    }
                }
            }
            return true;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (!Framing.GetTileSafely(i, j + 1).active() && !Framing.GetTileSafely(i - 1, j).active() && !Framing.GetTileSafely(i + 1, j).active())
                WorldGen.KillTile(i, j, false, false, false);

            if (Framing.GetTileSafely(i, j).frameX == 198 && Main.rand.Next(50) == 0)
                Gore.NewGore((new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-56, 56), Main.rand.Next(-44, 44) - 66), new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), mod.GetGoreSlot("Gores/Verdant/LushLeaf"));
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Tile t = Framing.GetTileSafely(i, j);

            if (fail)
                return;

            //if you value your sanity don't bother reading this

            int[] rootFrames = new int[] { 0, 18, 54, 72, 216, 234, 108, 126 };
            if (Helper.ActiveType(i, j - 1, Type) && !rootFrames.Contains(t.frameX))
                WorldGen.KillTile(i, j - 1, fail, false, false);

            if (t.frameX == 0 && Helper.ActiveType(i + 1, j, Type)) //Leftmost root
                Framing.GetTileSafely(i + 1, j).frameX = 216;
            else if (t.frameX == 72 && Helper.ActiveType(i - 1, j, Type)) //Rightmost root
                Framing.GetTileSafely(i - 1, j).frameX = 234;
            else if (t.frameX == 18 || t.frameX == 216) //Left root & cut left root
            {
                if (t.frameX == 18)
                    WorldGen.KillTile(i - 1, j, fail, false, false);

                if (Framing.GetTileSafely(i + 1, j).frameX == 36)
                    Framing.GetTileSafely(i + 1, j).frameX = 270;
                else if (Framing.GetTileSafely(i + 1, j).frameX == 252)
                    Framing.GetTileSafely(i + 1, j).frameX = 90;
            }
            else if (t.frameX == 54 || t.frameX == 234) //Right root & cut right root
            {
                if (t.frameX == 54)
                    WorldGen.KillTile(i + 1, j, fail, false, false);

                if (Framing.GetTileSafely(i - 1, j).frameX == 36)
                    Framing.GetTileSafely(i - 1, j).frameX = 252;
                else if (Framing.GetTileSafely(i - 1, j).frameX == 270)
                    Framing.GetTileSafely(i - 1, j).frameX = 90;
            }
            else if (t.frameX == 36)
            {
                WorldGen.KillTile(i - 1, j, false, false, false);
                WorldGen.KillTile(i + 1, j, false, false, false);
            }
            else if (t.frameX == 90 || t.frameX == 144 || t.frameX == 162 || t.frameX == 180 || t.frameX == 198 || t.frameX == 288 || t.frameX == 306 || t.frameX == 324) //Main tree cut
            {
                int nFrameX = Framing.GetTileSafely(i, j + 1).frameX;
                if (nFrameX == 90) Framing.GetTileSafely(i, j + 1).frameX = 288;
                if (nFrameX == 144) Framing.GetTileSafely(i, j + 1).frameX = 306;
                if (nFrameX == 162) Framing.GetTileSafely(i, j + 1).frameX = 324;
            }
            else if (t.frameX == 252)
                WorldGen.KillTile(i - 1, j, fail, false, false);
            else if (t.frameX == 270)
                WorldGen.KillTile(i + 1, j, fail, false, false);
            else if (t.frameX == 108)
            {
                if (Framing.GetTileSafely(i + 1, j).frameX == 162)
                    Framing.GetTileSafely(i + 1, j).frameX = 90;
                else
                    Framing.GetTileSafely(i + 1, j).frameX = 324;
            }
            else if (t.frameX == 126)
            {
                if (Framing.GetTileSafely(i + 1, j).frameX == 144)
                    Framing.GetTileSafely(i + 1, j).frameX = 90;
                else
                    Framing.GetTileSafely(i + 1, j).frameX = 306;
            }

            if (!fail && Framing.GetTileSafely(i, j + 2).active() && !Framing.GetTileSafely(i - 1, j - 1).active() && !Framing.GetTileSafely(i + 1, j - 1).active())
            {
                if (Framing.GetTileSafely(i - 1, j + 1).type == Type)
                    Framing.GetTileSafely(i - 1, j).frameX = 180;
                else
                    WorldGen.KillTile(i - 1, j);
            }

            if (Framing.GetTileSafely(i, j).frameX == 198) //gore stuff
            {
                int rnd = Main.rand.Next(8, 14);
                if (fail)
                    rnd = Main.rand.Next(2, 6);
                for (int l = 0; l < rnd; ++l)
                    Gore.NewGore((new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-56, 56), Main.rand.Next(-44, 44) - 66), new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), mod.GetGoreSlot("Gores/Verdant/LushLeaf"));
                if (!fail)
                {
                    int tot = Main.rand.Next(6, 11);
                    for (int k = 0; k < tot; ++k)
                        Item.NewItem((new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-46, 46), Main.rand.Next(-40, 40) - 66), ModContent.ItemType<VerdantWoodBlock>(), Main.rand.Next(1, 5));
                    tot = Main.rand.Next(1, 4);
                    for (int k = 0; k < tot; ++k)
                        Item.NewItem((new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-46, 46), Main.rand.Next(-40, 40) - 66), ItemID.Acorn, Main.rand.Next(1, 5));
                }
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile t = Framing.GetTileSafely(i, j);
            Texture2D tex = ModContent.GetTexture("Verdant/Tiles/Verdant/Trees/VerdantTree");
            Color col = Lighting.GetColor(i, j);
            float xOff = (float)Math.Sin((j * 19) * 0.04f) * 1.2f;
            if (xOff == 1 && (j / 4f) == 0)
                xOff = 0;

            int frameSize = 16;
            int frameOff = 0;
            int frameSizY = 16;
            if (t.frameX == 108 || t.frameX < 36 || t.frameX == 216 || t.frameX == 270) frameSize = 18;
            if (t.frameX == 126 || t.frameX == 52 || t.frameX == 72 || t.frameX == 232 || t.frameX == 252)
            {
                frameSize = 18;
                frameOff = -2;
            }
            if (t.frameX < 90 || t.frameX == 216 || t.frameX == 234 || t.frameX == 252 || t.frameX == 270) frameSizY = 18;
            Vector2 pos = Helper.TileCustomPosition(i, j) - new Vector2((xOff * 2) - (frameOff / 2), 0);

            if (Framing.GetTileSafely(i, j).frameX == 108) //Draw branches so it has to do less logic later
            {
                Texture2D tops = ModContent.GetTexture("Verdant/Tiles/Verdant/Trees/VerdantTreeBranches");
                int frame = t.frameY / 18;
                spriteBatch.Draw(tops, pos, new Rectangle(0, 52 * frame, 56, 50), new Color(col.R, col.G, col.B, 255), 0f, new Vector2(38, 16), 1f, SpriteEffects.None, 0f);
                return false;
            }

            if (Framing.GetTileSafely(i, j).frameX == 126) //Draw branches so it has to do less logic later
            {
                Texture2D tops = ModContent.GetTexture("Verdant/Tiles/Verdant/Trees/VerdantTreeBranches");
                int frame = t.frameY / 18;
                spriteBatch.Draw(tops, pos, new Rectangle(58, 52 * frame, 56, 50), new Color(col.R, col.G, col.B, 255), 0f, new Vector2(4, 16), 1f, SpriteEffects.None, 0f);
                return false;
            }

            spriteBatch.Draw(tex, pos, new Rectangle(t.frameX + frameOff, t.frameY, frameSize, frameSizY), new Color(col.R, col.G, col.B, 255), 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

            if (Framing.GetTileSafely(i, j).frameX == 198)
            {
                Texture2D tops = ModContent.GetTexture("Verdant/Tiles/Verdant/Trees/VerdantTreeTops");
                col = Lighting.GetColor(i, j - 2);
                int frame = t.frameY / 18;
                float rot = (float)Math.Sin((Main.time * 0.03f) + (i * 25)) * 0.02f;
                spriteBatch.Draw(tops, Helper.TileCustomPosition(i, j), new Rectangle(98 * frame, 0, 96, 96), new Color(col.R, col.G, col.B, 255), rot, new Vector2(40, 96), 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
