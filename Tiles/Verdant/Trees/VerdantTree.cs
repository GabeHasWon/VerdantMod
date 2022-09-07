using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Items.Verdant.Food;
using Verdant.Items.Verdant.Materials;
using Verdant.NPCs.Passive;

namespace Verdant.Tiles.Verdant.Trees
{
    internal class VerdantTree : ModTile
    {
        public override void SetStaticDefaults()
        {
            QuickTile.SetAll(this, 0, DustID.t_BorealWood, SoundID.Dig, new Color(142, 62, 32), ModContent.ItemType<VerdantWoodBlock>(), "Tree", false, false, false, false);
            Main.tileFrameImportant[Type] = true;
            Main.tileAxe[Type] = true;

            TileID.Sets.IsATreeTrunk[Type] = true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = (fail ? 1 : 3);

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            resetFrame = false;
            noBreak = true;
            return false;
        }

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
                if (TileHelper.SolidTile(i, j - k))
                {
                    height = k - 2;
                    break;
                }
            }

            if (height < 4 || height < minSize) 
                return false;

            bool[] extraPlaces = new bool[5];
            for (int k = -2; k < 3; ++k) //Checks base
            {
                extraPlaces[k + 2] = false;
                if ((TileHelper.SolidTopTile(i + k, j + 1) || TileHelper.SolidTile(i + k, j + 1)) && !Framing.GetTileSafely(i + k, j).HasTile)
                    extraPlaces[k + 2] = true;
            }

            if (!extraPlaces[1]) extraPlaces[0] = false;
            if (!extraPlaces[3]) extraPlaces[4] = false;

            if (!extraPlaces[2]) return false;

            if (j > Main.worldSurface) //Base only exists on surface
                extraPlaces = new bool[5] { false, false, true, false, false };

            for (int k = -2; k < 3; ++k) //Places base
            {
                if (extraPlaces[k + 2])
                    WorldGen.PlaceTile(i + k, j, type, true);
                else
                    continue;

                Framing.GetTileSafely(i + k, j).TileFrameX = (short)((k + 2) * 18);
                Framing.GetTileSafely(i + k, j).TileFrameY = (short)(r.Next(3) * 18);

                if (!extraPlaces[0] && k == -1)
                    Framing.GetTileSafely(i + k, j).TileFrameX = 216;
                if (!extraPlaces[3] && k == 1)
                    Framing.GetTileSafely(i + k, j).TileFrameX = 234;

                if (!extraPlaces[1] && !extraPlaces[3] && k == 0) 
                    Framing.GetTileSafely(i + k, j).TileFrameX = 90;
                if (extraPlaces[1] && !extraPlaces[3] && k == 0) 
                    Framing.GetTileSafely(i + k, j).TileFrameX = 252;
                if (!extraPlaces[1] && extraPlaces[3] && k == 0) 
                    Framing.GetTileSafely(i + k, j).TileFrameX = 270;
            }

            for (int k = 1; k < height; ++k)
            {
                WorldGen.PlaceTile(i, j - k, type, true);
                Framing.GetTileSafely(i, j - k).TileFrameX = 90;
                Framing.GetTileSafely(i, j - k).TileFrameY = (short)(r.Next(3) * 18);

                if (k == height - 1)
                {
                    if (r.NextBool(12)) 
                        Framing.GetTileSafely(i, j - k).TileFrameX = 180;
                    else 
                        Framing.GetTileSafely(i, j - k).TileFrameX = 198;
                }
                else if (r.NextBool(4))
                {
                    int side = r.Next(2);
                    if (side == 0 && !Framing.GetTileSafely(i - 1, j - k).HasTile)
                    {
                        WorldGen.PlaceTile(i - 1, j - k, type, true);
                        Framing.GetTileSafely(i, j - k).TileFrameX = 162;
                        Framing.GetTileSafely(i - 1, j - k).TileFrameX = 108;
                        Framing.GetTileSafely(i - 1, j - k).TileFrameY = (short)(r.Next(3) * 18);
                    }
                    else if (side == 1 && !Framing.GetTileSafely(i + 1, j - k).HasTile)
                    {
                        WorldGen.PlaceTile(i + 1, j - k, type, true);
                        Framing.GetTileSafely(i, j - k).TileFrameX = 144;
                        Framing.GetTileSafely(i + 1, j - k).TileFrameX = 126;
                        Framing.GetTileSafely(i + 1, j - k).TileFrameY = (short)(r.Next(3) * 18);
                    }
                }

                if (leaves)
                {
                    if (r.Next(4) <= 1)
                    {
                        int rnd = r.Next(2, 5);
                        for (int l = 0; l < rnd; ++l)
                            Gore.NewGore(Entity.GetSource_NaturalSpawn(), (new Vector2(i, j - k) * 16) + new Vector2(8 + r.Next(-4, 5), 8), new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), leavesType);
                    }
                }
            }
            return true;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (!Framing.GetTileSafely(i, j + 1).HasTile && !Framing.GetTileSafely(i - 1, j).HasTile && !Framing.GetTileSafely(i + 1, j).HasTile)
                WorldGen.KillTile(i, j, false, false, false);

            if (Framing.GetTileSafely(i, j).TileFrameX == 198 && Main.rand.NextBool(120))
                Gore.NewGore(new EntitySource_TileUpdate(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-56, 56), Main.rand.Next(-44, 44) - 66), new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("LushLeaf").Type);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Tile t = Framing.GetTileSafely(i, j);

            if (fail)
            {
                (int x, int y) = (i, j);
                ClimbToTop(ref x, ref y);
            }

            if (Framing.GetTileSafely(i, j).TileFrameX == 198) //gore stuff
            {
                int rnd = Main.rand.Next(8, 14);
                if (fail)
                    rnd = Main.rand.Next(2, 6);
                for (int l = 0; l < rnd; ++l)
                    Gore.NewGore(new EntitySource_TileBreak(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-56, 56), Main.rand.Next(-44, 44) - 66), new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("LushLeaf").Type);
                if (!fail)
                {
                    int tot = Main.rand.Next(6, 11);
                    for (int k = 0; k < tot; ++k)
                        Item.NewItem(new EntitySource_TileBreak(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-46, 46), Main.rand.Next(-40, 40) - 66), ModContent.ItemType<VerdantWoodBlock>(), Main.rand.Next(1, 5));
                    tot = Main.rand.Next(1, 4);
                    for (int k = 0; k < tot; ++k)
                        Item.NewItem(new EntitySource_TileBreak(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-46, 46), Main.rand.Next(-40, 40) - 66), ItemID.Acorn, Main.rand.Next(1, 5));
                    tot = Main.rand.Next(3, 8);
                    for (int k = 0; k < tot; ++k)
                        Item.NewItem(new EntitySource_TileBreak(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-46, 46), Main.rand.Next(-40, 40) - 66), ModContent.ItemType<LushLeaf>(), Main.rand.Next(1, 5));
                }
            }

            if (Framing.GetTileSafely(i, j).TileFrameX == 108 || Framing.GetTileSafely(i, j).TileFrameX == 126)
            {
                int side = Framing.GetTileSafely(i, j).TileFrameX == 108 ? -1 : 1;
                if (!fail) //kill
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(40) * side, Main.rand.Next(-10, 10)), ModContent.ItemType<LushLeaf>(), Main.rand.Next(3, 8));
                    Item.NewItem(new EntitySource_TileBreak(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(40) * side, Main.rand.Next(-10, 10)), ItemID.Acorn, Main.rand.Next(1, 3));
                    int rnd = Main.rand.Next(8, 14);
                    for (int l = 0; l < rnd; ++l)
                        Gore.NewGore(new EntitySource_TileBreak(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(40) * side, Main.rand.Next(-10, 10)), new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("LushLeaf").Type);
                }
                else
                {
                    int rnd = Main.rand.Next(1, 4);
                    for (int l = 0; l < rnd; ++l)
                        Gore.NewGore(new EntitySource_TileBreak(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(40) * side, Main.rand.Next(-10, 10)), new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("LushLeaf").Type);
                    if (Main.rand.NextBool(8))
                        Item.NewItem(new EntitySource_TileBreak(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(40) * side, Main.rand.Next(-10, 10)), ModContent.ItemType<LushLeaf>(), Main.rand.Next(1, 3));
                }
            }

            if (fail)
                return;

            //if you value your sanity don't bother reading this

            int[] rootFrames = new int[] { 0, 18, 54, 72, 216, 234, 108, 126 };
            if (TileHelper.ActiveType(i, j - 1, Type) && !rootFrames.Contains(t.TileFrameX))
                WorldGen.KillTile(i, j - 1, fail, false, false);

            if (t.TileFrameX == 0 && TileHelper.ActiveType(i + 1, j, Type)) //Leftmost root
                Framing.GetTileSafely(i + 1, j).TileFrameX = 216;
            else if (t.TileFrameX == 72 && TileHelper.ActiveType(i - 1, j, Type)) //Rightmost root
                Framing.GetTileSafely(i - 1, j).TileFrameX = 234;
            else if (t.TileFrameX == 18 || t.TileFrameX == 216) //Left root & cut left root
            {
                if (t.TileFrameX == 18)
                    WorldGen.KillTile(i - 1, j, fail, false, false);

                if (Framing.GetTileSafely(i + 1, j).TileFrameX == 36)
                    Framing.GetTileSafely(i + 1, j).TileFrameX = 270;
                else if (Framing.GetTileSafely(i + 1, j).TileFrameX == 252)
                    Framing.GetTileSafely(i + 1, j).TileFrameX = 90;
            }
            else if (t.TileFrameX == 54 || t.TileFrameX == 234) //Right root & cut right root
            {
                if (t.TileFrameX == 54)
                    WorldGen.KillTile(i + 1, j, fail, false, false);

                if (Framing.GetTileSafely(i - 1, j).TileFrameX == 36)
                    Framing.GetTileSafely(i - 1, j).TileFrameX = 252;
                else if (Framing.GetTileSafely(i - 1, j).TileFrameX == 270)
                    Framing.GetTileSafely(i - 1, j).TileFrameX = 90;
            }
            else if (t.TileFrameX == 36)
            {
                WorldGen.KillTile(i - 1, j, false, false, false);
                WorldGen.KillTile(i + 1, j, false, false, false);
            }
            else if (t.TileFrameX == 90 || t.TileFrameX == 144 || t.TileFrameX == 162 || t.TileFrameX == 180 || t.TileFrameX == 198 || t.TileFrameX == 288 || t.TileFrameX == 306 || t.TileFrameX == 324) //Main tree cut
            {
                int nFrameX = Framing.GetTileSafely(i, j + 1).TileFrameX;
                if (nFrameX == 90) Framing.GetTileSafely(i, j + 1).TileFrameX = 288;
                if (t.TileFrameX == 144) //right branch
                {
                    WorldGen.KillTile(i + 1, j, fail);
                    Framing.GetTileSafely(i, j + 1).TileFrameX = 306;
                }
                if (nFrameX == 162) Framing.GetTileSafely(i, j + 1).TileFrameX = 324;
            }
            else if (t.TileFrameX == 252)
                WorldGen.KillTile(i - 1, j, fail, false, false);
            else if (t.TileFrameX == 270)
                WorldGen.KillTile(i + 1, j, fail, false, false);
            else if (t.TileFrameX == 108)
            {
                if (Framing.GetTileSafely(i + 1, j).TileFrameX == 162)
                    Framing.GetTileSafely(i + 1, j).TileFrameX = 90;
                else
                    Framing.GetTileSafely(i + 1, j).TileFrameX = 324;
            }
            else if (t.TileFrameX == 126)
            {
                if (Framing.GetTileSafely(i + 1, j).TileFrameX == 144)
                    Framing.GetTileSafely(i + 1, j).TileFrameX = 90;
                else
                    Framing.GetTileSafely(i + 1, j).TileFrameX = 306;
            }

            if (!fail && Framing.GetTileSafely(i, j + 2).HasTile && !Framing.GetTileSafely(i - 1, j - 1).HasTile && !Framing.GetTileSafely(i + 1, j - 1).HasTile)
            {
                if (Framing.GetTileSafely(i - 1, j + 1).TileType == Type)
                    Framing.GetTileSafely(i - 1, j).TileFrameX = 180;
                else
                    WorldGen.KillTile(i - 1, j);
            }
        }

        private void ClimbToTop(ref int x, ref int y)
        {
            while (Main.tile[x, y].HasTile && Main.tile[x, y].TileType == Type)
                y--;
            y++;

            if (Main.tile[x, y].TileFrameX == 198)
            {
                for (int k = 0; k < WorldGen.numTreeShakes; k++)
                    if (WorldGen.treeShakeX[k] == x && WorldGen.treeShakeY[k] == y)
                        return;

                WorldGen.treeShakeX[WorldGen.numTreeShakes] = x;
                WorldGen.treeShakeY[WorldGen.numTreeShakes] = y;
                WorldGen.numTreeShakes++;

                WeightedRandom<int> random = new WeightedRandom<int>(Main.rand);

                random.Add(0, 1);
                random.Add(1, 0.7f);
                random.Add(2, 0.85f);

                int rand = random;
                if (rand == 1)
                {
                    int type = Main.rand.NextBool() ? ModContent.ItemType<Dropberry>() : ModContent.ItemType<BowlFruit>();
                    Item.NewItem(new EntitySource_TileInteraction(Main.LocalPlayer, x, y), (new Vector2(x, y) * 16) + new Vector2(Main.rand.Next(-56, 56), Main.rand.Next(-44, 44) - 66), type, Main.rand.Next(1, 4));
                }
                else if (rand == 2)
                {
                    int reps = Main.rand.Next(3, 6);

                    WeightedRandom<int> npcType = new(Main.rand);
                    npcType.Add(ModContent.NPCType<Flotie>(), 0.9f);
                    npcType.Add(ModContent.NPCType<Flotiny>(), 1.2f);
                    npcType.Add(ModContent.NPCType<VerdantBulbSnail>(), 1f);
                    npcType.Add(ModContent.NPCType<VerdantRedGrassSnail>(), 1f);

                    for (int i = 0; i < reps; ++i)
                        NPC.NewNPC(new EntitySource_TileInteraction(Main.LocalPlayer, x, y), x * 16, y * 16, npcType);
                }

                for (int i = 0; i < 20; ++i)
                    Gore.NewGore(new EntitySource_TileUpdate(x, y), (new Vector2(x, y) * 16) + new Vector2(Main.rand.Next(-56, 56), Main.rand.Next(-44, 44) - 66), new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("LushLeaf").Type);
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile t = Framing.GetTileSafely(i, j);
            Texture2D tex = ModContent.Request<Texture2D>("Verdant/Tiles/Verdant/Trees/VerdantTree").Value;
            Color col = Lighting.GetColor(i, j);
            float xOff = (float)Math.Sin((j * 19) * 0.04f) * 1.2f;
            if (xOff == 1 && (j / 4f) == 0)
                xOff = 0;

            int frameSize = 16;
            int frameOff = 0;
            int frameSizY = 16;
            if (t.TileFrameX == 108 || t.TileFrameX < 36 || t.TileFrameX == 216 || t.TileFrameX == 270) frameSize = 18;
            if (t.TileFrameX == 126 || t.TileFrameX == 52 || t.TileFrameX == 72 || t.TileFrameX == 232 || t.TileFrameX == 252)
            {
                frameSize = 18;
                frameOff = -2;
            }
            if (t.TileFrameX < 90 || t.TileFrameX == 216 || t.TileFrameX == 234 || t.TileFrameX == 252 || t.TileFrameX == 270) frameSizY = 18;

            Vector2 offset =  new((xOff * 2) - (frameOff / 2), 0);
            Vector2 pos = TileHelper.TileCustomPosition(i, j) - offset;

            if (Framing.GetTileSafely(i, j).TileFrameX == 108) //Draw branches so it has to do less logic later
            {
                Texture2D tops = ModContent.Request<Texture2D>("Verdant/Tiles/Verdant/Trees/VerdantTreeBranches").Value;
                int frame = t.TileFrameY / 18;
                spriteBatch.Draw(tops, pos, new Rectangle(0, 52 * frame, 56, 50), new Color(col.R, col.G, col.B, 255), 0f, new Vector2(38, 16), 1f, SpriteEffects.None, 0f);
                return false;
            }

            if (Framing.GetTileSafely(i, j).TileFrameX == 126) //Draw branches so it has to do less logic later
            {
                Texture2D tops = ModContent.Request<Texture2D>("Verdant/Tiles/Verdant/Trees/VerdantTreeBranches").Value;
                int frame = t.TileFrameY / 18;
                spriteBatch.Draw(tops, pos, new Rectangle(58, 52 * frame, 56, 50), new Color(col.R, col.G, col.B, 255), 0f, new Vector2(4, 16), 1f, SpriteEffects.None, 0f);
                return false;
            }

            spriteBatch.Draw(tex, pos, new Rectangle(t.TileFrameX + frameOff, t.TileFrameY, frameSize, frameSizY), new Color(col.R, col.G, col.B, 255), 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

            if (Framing.GetTileSafely(i, j).TileFrameX == 198)
            {
                Texture2D tops = ModContent.Request<Texture2D>("Verdant/Tiles/Verdant/Trees/VerdantTreeTops").Value;
                int frame = t.TileFrameY / 18;

                TileSwaySystem.DrawTreeSway(i - 1, j - 1, tops, new Rectangle(98 * frame, 0, 96, 108), TileHelper.TileOffset.ToWorldCoordinates(), new Vector2(40, 96));
            }
            return false;
        }
    }
}
