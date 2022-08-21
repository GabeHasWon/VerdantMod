using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;
using Terraria.DataStructures;

namespace Verdant.Tiles.Verdant.Trees
{
    class BigVerdantTree : ModTile
    {
        private static int LeafType = GoreID.TreeLeaf_Normal; //Set this to whatever custom type you want. If not a constant, do it in SetDefaults
        /// <summary>X/Y are equal to the width & height (respectively), in tiles, the top is - then, the width and height are the width/height in pixels
        /// Note: The tops are draw by the origin of the middle-bottom of the trunk, so generally, you'd want to use TreeTopSize.X / 2 for the left side (and same for the right side)</summary>
        private readonly Rectangle TreeTopSize = new Rectangle(24, 20, 360, 320);
        /// <summary>X/Y are equal to the width & height (respectively), in tiles, the branch is - then, the width and height are the width/height in pixels
        /// Note: The tops are draw by the origin of the [side]-middle of the trunk, so generally, you'd want to use TreeTopSize.Y / 2 for the top side (and same for the bottom side)</summary>
        private readonly Rectangle BranchSize = new Rectangle(9, 4, 144, 64);

        public override void SetStaticDefaults()
        {
            MinPick = 0;
            DustType = DustID.t_LivingWood;
            HitSound = SoundID.Dig;
            ItemDrop = ItemID.Wood;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Big Tree");
            AddMapEntry(new Color(151, 107, 72), name);

            LeafType = Mod.Find<ModGore>("LushLeaf").Type;

            Main.tileMergeDirt[Type] = false; //This tree should never merge with anything - always keep this
            Main.tileSolid[Type] = false; //Does not have collision
            Main.tileLighted[Type] = false; //Does not give off light
            Main.tileBlockLight[Type] = false; //Does not block light
            Main.tileFrameImportant[Type] = true; //Save frame when world is exited
            Main.tileAxe[Type] = true; //Cut with axe
        }

        /// <summary>Spawns a tree at x, y, origin being the left side of the trunk.</summary>
        /// <param name="r">Pass WorldGen.genRand if you plan to use this during worldgen. Defaults to Main.rand.</param>
        /// <param name="height">Height of the tree in tiles.</param>
        /// <param name="requireHeight">If true, this tree will not grow in too small of a space. If false, this will fit the space.</param>
        /// <param name="minHeight">Stops the spawn if height is too low, or, if requireHeight is false, the adjusted height is too low.</param>
        public static void Spawn(int x, int y, int height, UnifiedRandom r = null, bool requireHeight = false, int minHeight = 8, bool spawnLeaves = true) 
        {
            //Feel free to move this method wherever; in my main project I have it under a TileHelper class that also takes a type param for brevity.
            r = r ?? Main.rand; //Defaults to Main.rand. Again, should be WorldGen.genRand if you want this to work in world generaiton.

            if (y < minHeight * 2) return; //There may not be enough space in the world for this, abort now

            for (int i = 1; i < height; ++i) //Checks height for obstructions so it doesn't grow into the ceiling
            {
                if ((Framing.GetTileSafely(x, y - i).HasTile && Main.tileSolid[Framing.GetTileSafely(x, y - i).TileType]) || (Framing.GetTileSafely(x + 1, y - i).HasTile && Main.tileSolid[Framing.GetTileSafely(x + 1, y - i).TileType]))
                {
                    if (!requireHeight) //Adjust height
                        height = i - 1;
                    else //Stop spawning if you require a specific height
                        return;
                }
            }

            if (height <= minHeight) return; //Tree is too small, stop the spawn

            bool FullTile(int i, int j) => Framing.GetTileSafely(i, j).HasTile && !Main.tileCut[Framing.GetTileSafely(i, j).TileType];//True if a tile is active & not overrideable
            bool SolidTile(int i, int j) => Framing.GetTileSafely(i, j).HasTile && Main.tileSolid[Framing.GetTileSafely(i, j).TileType]; //Self explanatory

            Place(x, y,     (short)(!FullTile(x - 1, y) && SolidTile(x - 1, y + 1) ? 72 : 0), (short)(18 * r.Next(3))); //Middle base
            Place(x + 1, y, (short)(!FullTile(x + 2, y) && SolidTile(x + 2, y + 1) ? 306 : 18), (short)(18 * r.Next(3)));
            if (!FullTile(x - 1, y) && SolidTile(x - 1, y + 1)) Place(x - 1, y, 36,  (short)(18 * r.Next(3))); //Left root if space is open & has footing
            if (!FullTile(x + 2, y) && SolidTile(x + 2, y + 1)) Place(x + 2, y, 54,  (short)(18 * r.Next(3))); //Right root if space is open & has footing

            int[] lastBranch = new int[2]; //Makes sure the gen doesn't spam branches

            for (int i = 1; i < height; ++i) //Trunk
            {
                Place(x, y - i,      0, (short)(18 * r.Next(3))); //Place trunk
                Place(x + 1, y - i, 18, (short)(18 * r.Next(3))); //Trunk, part 2

                if ((lastBranch[0] < 0 || lastBranch[1] < 0) && i > 3 && i < height - 2 && r.NextBool(2)) //Branch chance and conditions
                {
                    bool leaf = r.Next(4) > 0; //Leaf chance

                    //There's probably a cleaner way to do this but I didn't think of it.
                    bool left = r.NextBool(2); //Direction logic -->
                    if (lastBranch[0] > 0) left = false; //'Timer' checks
                    else if (lastBranch[1] > 0) left = true;
                    else if (lastBranch[0] < 0 && lastBranch[1] < 0) left = r.NextBool(2);

                    if (Framing.GetTileSafely(x - 1, y - i).HasTile || Framing.GetTileSafely(x - 2, y - i).HasTile && left) //Tile-based placement checks
                    {
                        if ((lastBranch[1] > 0)) goto skipBranch; //If this branch cannot, in any way, be placed, skip
                        left = false; //Adjust side
                    }
                    if (Framing.GetTileSafely(x + 2, y - i).HasTile || Framing.GetTileSafely(x + 3, y - i).HasTile && !left) //Tile-based placement checks
                    {
                        if ((lastBranch[0] > 0)) goto skipBranch; //If this branch cannot, in any way, be placed, skip
                        left = true; //Adjust side
                    }
                    if ((Framing.GetTileSafely(x - 1, y - i).HasTile || Framing.GetTileSafely(x - 2, y - i).HasTile) && //If both placements are invalid, skip this entirely
                        (Framing.GetTileSafely(x + 2, y - i).HasTile || Framing.GetTileSafely(x + 3, y - i).HasTile)) goto skipBranch; //Eww goto but I didn't wanna reformat this <--

                    if (left) //Place left branch; adjust trunk
                    {
                        Framing.GetTileSafely(x, y - i).TileFrameX = 198;
                        Place(x - 1, y - i, 90, (short)(18 * r.Next(3)));
                        Place(x - 2, y - i, (short)(leaf ? 270 : 108), (short)(18 * r.Next(3)));

                        lastBranch[0] = 3 + Main.rand.Next(2); //Make them unable to place within 3-4 tiles of each other. You can change this as you please.
                    }
                    else //Same as lines 68-75, but for right branch
                    {
                        Framing.GetTileSafely(x + 1, y - i).TileFrameX = 216;
                        Place(x + 2, y - i, 144, (short)(18 * r.Next(3)));
                        Place(x + 3, y - i, (short)(leaf ? 288 : 126), (short)(18 * r.Next(3)));

                        lastBranch[1] = 3 + Main.rand.Next(2);
                    }

                    int leaves = r.Next(3, 6); //Random leaf spawns for trunk
                    if (spawnLeaves) //Spawns leaf gores
                        for (int l = 0; l < leaves; ++l)
                            Gore.NewGore(Entity.GetSource_NaturalSpawn(), (new Vector2(x + r.Next(2), y - i) * 16) + new Vector2(r.Next(16), r.Next(16)), new Vector2(), LeafType, r.Next(90, 130) * 0.01f);
                }

                skipBranch: //Case for skipping branch placement
                lastBranch[0]--; //Branch countdown
                lastBranch[1]--; //Branch countdown 2

                if (i == height - 1) //Treetop
                {
                    Framing.GetTileSafely(x, y - i).TileFrameX = 234; //Treetop tiles for drawing the tops
                    Framing.GetTileSafely(x + 1, y - i).TileFrameX = 252;
                    int leaves = r.Next(20, 28);
                    if (spawnLeaves) //Spawns leaf gores
                        for (int l = 0; l < leaves; ++l)
                            Gore.NewGore(Entity.GetSource_NaturalSpawn(), (new Vector2(x - 12, y - i - 20) * 16) + new Vector2(r.Next(360), r.Next(320)), new Vector2(), LeafType, r.Next(120, 160) * 0.01f);
                }
            }
        }

        /// <summary>Just makes it easier to place tiles with good framing.</summary>
        private static void Place(int x, int y, short frameX, short frameY)
        {
            WorldGen.PlaceTile(x, y, ModContent.TileType<BigVerdantTree>(), true, false, -1, 0);
            Framing.GetTileSafely(x, y).TileFrameX = frameX;
            Framing.GetTileSafely(x, y).TileFrameY = frameY;
        }

        /// <summary>Used to make sure associated tiles on the tree die when any given tile is killed.
        /// Note: This is a very long checklist of every possible way to destroy a tile; in short, custom framing. If you want to change something, you might have to rewrite a good amount of it.</summary>
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail) return; //Do nothing if this doesn't kill the tile

            Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, ItemDrop, Main.rand.Next(1, 3)); //Drops extra wood to be proportional to vanilla tree wood loot

            Tile t = Framing.GetTileSafely(i, j); //For quick reference

            if (Framing.GetTileSafely(i, j - 1).TileType == Type) //Kill any tree tiles above self
                WorldGen.KillTile(i, j - 1);

            if (t.TileFrameX == 108 || t.TileFrameX == 270) Framing.GetTileSafely(i + 1, j).TileFrameX = 162; //Adjust frame for the middle branch
            if (t.TileFrameX == 126 || t.TileFrameX == 288) Framing.GetTileSafely(i - 1, j).TileFrameX = 180; //Same as above, but the right side

            if (t.TileFrameX == 90) //[Left] Middle branch killed, kill branch tip and adjust trunk frame
            {
                Framing.GetTileSafely(i + 1, j).TileFrameX = 0;
                WorldGen.KillTile(i - 1, j);
            }
            if (t.TileFrameX == 162) Framing.GetTileSafely(i + 1, j).TileFrameX = 0; //Adjust trunk when cut branch is killed
            if (t.TileFrameX == 144) //[Right] Middle branch killed, kill branch tip and adjust trunk frame
            {
                Framing.GetTileSafely(i - 1, j).TileFrameX = 18;
                WorldGen.KillTile(i + 1, j);
            }
            if (t.TileFrameX == 180) Framing.GetTileSafely(i - 1, j).TileFrameX = 18; //Adjust trunk when cut branch is killed

            if (t.TileFrameX == 36) Framing.GetTileSafely(i + 1, j).TileFrameX = 0; //Base left side, adjust trunk frame
            if (t.TileFrameX == 54) Framing.GetTileSafely(i - 1, j).TileFrameX = 18; //Base right side, adjust trunk frame

            if (t.TileFrameX == 198 || t.TileFrameX == 396) //If adjacent to a branch, kill the branch
                WorldGen.KillTile(i - 1, j, false, false, false);
            if (t.TileFrameX == 216 || t.TileFrameX == 414) //If adjacent to a branch, kill the branch
                WorldGen.KillTile(i + 1, j, false, false, false);

            if (t.TileFrameX == 72 || t.TileFrameX == 324) //Base is killed
            {
                t.TileFrameX++;
                WorldGen.KillTile(i - 1, j, false, false, false); //Kills left base root
                if (Framing.GetTileSafely(i + 1, j).TileFrameX == 306 || Framing.GetTileSafely(i + 1, j).TileFrameX == 342)
                    WorldGen.KillTile(i + 1, j, false, false, false); //Kills the rest of the base
            }
            if (t.TileFrameX == 306 || t.TileFrameX == 342) //Base is killed
            {
                t.TileFrameX++;
                WorldGen.KillTile(i + 1, j, false, false, false); //Kills right base root
                if (Framing.GetTileSafely(i - 1, j).TileFrameX == 72 || Framing.GetTileSafely(i + 1, j).TileFrameX == 324)
                    WorldGen.KillTile(i - 1, j, false, false, false); //Kills rest of the base
            }

            void CutTrunkFrames(int adj) //Cuts the trunk. Please help me
            {
                if (Framing.GetTileSafely(i - adj, j + 1).TileFrameX == 0)
                    Framing.GetTileSafely(i - adj, j + 1).TileFrameX = 360; //Cut trunk
                if (Framing.GetTileSafely(i - adj, j + 1).TileFrameX == 198)
                    Framing.GetTileSafely(i - adj, j + 1).TileFrameX = 396; //Cut trunk w/ adjacent branch
                if (Framing.GetTileSafely(i - adj, j + 1).TileFrameX == 72)
                    Framing.GetTileSafely(i - adj, j + 1).TileFrameX = 324;
                if (Framing.GetTileSafely(i - adj + 1, j + 1).TileFrameX == 18)
                    Framing.GetTileSafely(i - adj + 1, j + 1).TileFrameX = 378; //Cut trunk (right)
                if (Framing.GetTileSafely(i - adj + 1, j + 1).TileFrameX == 216)
                    Framing.GetTileSafely(i - adj + 1, j + 1).TileFrameX = 414; //Cut trunk w/ adjacent branch (right)
                if (Framing.GetTileSafely(i - adj + 1, j + 1).TileFrameX == 306)
                    Framing.GetTileSafely(i - adj + 1, j + 1).TileFrameX = 342;
            }

            if (t.TileFrameX == 0 || t.TileFrameX == 198 || t.TileFrameX == 360) //Kill adjacent tiles if this is a trunk or the very base of the tree - left
            {
                t.TileFrameX++; //This makes sure there's no infinite loop without messing with anything
                int[] killAdj = new int[] { 18, 216, 306, 342, 378, 414 };
                if (killAdj.Any(x => Framing.GetTileSafely(i + 1, j).TileFrameX == x)) //Kills adjacent tile, w/ conditions to avoid infinite loop
                    WorldGen.KillTile(i + 1, j, false, false, false);
                CutTrunkFrames(0); //Cuts the trunk
            }
            if (t.TileFrameX == 18 || t.TileFrameX == 216 || t.TileFrameX == 378) //This is the same as the if above - right
            {
                t.TileFrameX++;
                int[] killAdj = new int[] { 0, 72, 198, 324, 360, 396 };
                if (killAdj.Any(x => Framing.GetTileSafely(i - 1, j).TileFrameX == x))
                    WorldGen.KillTile(i - 1, j, false, false, false);
                CutTrunkFrames(1);
            }

            if (t.TileFrameX == 360) //Check #29907832: killed adjacent cut trunk
            {
                t.TileFrameX++;
                if (Framing.GetTileSafely(i + 1, j).TileFrameX == 378) WorldGen.KillTile(i + 1, j);
            }
            if (t.TileFrameX == 378)
            {
                t.TileFrameX++;
                if (Framing.GetTileSafely(i - 1, j).TileFrameX == 360) WorldGen.KillTile(i - 1, j);
            }

            if (t.TileFrameX == 234) //Drops loot & gores from the treetop
            {
                Vector2 topPos = new Vector2(i - (TreeTopSize.X / 2), j - TreeTopSize.Y) * 16f;
                Item.NewItem(new EntitySource_TileBreak(i, j), topPos + new Vector2(Main.rand.Next(TreeTopSize.Width), Main.rand.Next(TreeTopSize.Height)), ItemDrop, Main.rand.Next(18, 25)); //Replace Acorn (below) with whatever if you want
                Item.NewItem(new EntitySource_TileBreak(i, j), topPos + new Vector2(Main.rand.Next(TreeTopSize.Width), Main.rand.Next(TreeTopSize.Height)), ItemID.Acorn, Main.rand.Next(7, 12)); //And obviously, add whatever extra items too if needed
                int leaves = Main.rand.Next(20, 28); //Randomized leaf count
                for (int l = 0; l < leaves; ++l) //Spawns leaf gores - note, these values are adjusted specifically for the size of the top; change TreeTopSize to adjust it easily.
                    Gore.NewGore(new EntitySource_TileBreak(i, j), topPos + new Vector2(Main.rand.Next(TreeTopSize.Width), Main.rand.Next(TreeTopSize.Height)), new Vector2(), LeafType, Main.rand.Next(120, 160) * 0.01f);

                t.TileFrameX++;
                if (Framing.GetTileSafely(i + 1, j).TileFrameX == 252) WorldGen.KillTile(i + 1, j, false, false, false);

                CutTrunkFrames(0); //Cuts the trunk
            }
            if (t.TileFrameX == 252) //Right side top, kill adjacent
            {
                t.TileFrameX++;
                if (Framing.GetTileSafely(i - 1, j).TileFrameX == 234) WorldGen.KillTile(i - 1, j, false, false, false);
                CutTrunkFrames(1); //Cuts the trunk
            }
            if (t.TileFrameX == 270) //Drops loot & gores from branch (left)
            {
                Vector2 branchPos = new Vector2(i - BranchSize.X, j - (BranchSize.Y / 2)) * 16;
                Item.NewItem(new EntitySource_TileBreak(i, j), branchPos + new Vector2(Main.rand.Next(BranchSize.Width), Main.rand.Next(BranchSize.Height)), ItemID.Acorn, Main.rand.Next(3, 5));
                int leaves = Main.rand.Next(12, 22);
                for (int l = 0; l < leaves; ++l)
                    Gore.NewGore(new EntitySource_TileBreak(i, j), branchPos + new Vector2(Main.rand.Next(BranchSize.Width), Main.rand.Next(BranchSize.Height)), new Vector2(), LeafType, Main.rand.Next(90, 130) * 0.01f);
            }
            if (t.TileFrameX == 288) //Drops loot & gores from branch (left)
            {
                Vector2 branchPos = new Vector2(i, j - (BranchSize.Y / 2)) * 16;
                Item.NewItem(new EntitySource_TileBreak(i, j), branchPos + new Vector2(Main.rand.Next(BranchSize.Width), Main.rand.Next(BranchSize.Height)), ItemID.Acorn, Main.rand.Next(3, 5));
                int leaves = Main.rand.Next(12, 22);
                for (int l = 0; l < leaves; ++l)
                    Gore.NewGore(new EntitySource_TileBreak(i, j), branchPos + new Vector2(Main.rand.Next(BranchSize.Width), Main.rand.Next(BranchSize.Height)), new Vector2(), LeafType, Main.rand.Next(90, 130) * 0.01f);
            }
        }

        /// <summary>Used to draw the tops & branches. Could be replaced to </summary>
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color col = Lighting.GetColor(i, j - 1); //Color for the treetop and branches
            int frame = Framing.GetTileSafely(i, j).TileFrameY / 18;
            
            if (Framing.GetTileSafely(i, j).TileFrameX == 234) //Tops
            {
                Texture2D tops = ModContent.Request<Texture2D>("Verdant/Tiles/Verdant/Trees/BigVerdantTree_Tops").Value; //Texture --- For the below line, it draws by x = middle, y = bottom origin. Adjust at your own risk
                spriteBatch.Draw(tops, TileCustomPosition(i, j), new Rectangle(0, 330 * frame, 360, 328), new Color(col.R, col.G, col.B, 255), 0f, new Vector2(162, 328), 1f, SpriteEffects.None, 0f);
                return true; //We still want to draw the connecting trunk
            }

            if (Framing.GetTileSafely(i, j).TileFrameX == 270) //Branch (left)
            {
                Texture2D tops = ModContent.Request<Texture2D>("Verdant/Tiles/Verdant/Trees/BigVerdantTree_Branches").Value; //Texture --- For the below line, it draws by x = right side, y = middle origin. Adjust at your own risk
                spriteBatch.Draw(tops, TileCustomPosition(i, j), new Rectangle(0, 144 * frame, 144, 144), new Color(col.R, col.G, col.B, 255), 0f, new Vector2(126, 68), 1f, SpriteEffects.None, 0f);
                return false; //Do not draw the default branch under this
            }

            if (Framing.GetTileSafely(i, j).TileFrameX == 288) //Branch (right)
            {
                Texture2D tops = ModContent.Request<Texture2D>("Verdant/Tiles/Verdant/Trees/BigVerdantTree_Branches").Value; //Texture --- For the below line, it draws by x = left side, y = middle origin. Adjust at your own risk
                spriteBatch.Draw(tops, TileCustomPosition(i, j), new Rectangle(146, 144 * frame, 144, 144), new Color(col.R, col.G, col.B, 255), 0f, new Vector2(0, 68), 1f, SpriteEffects.None, 0f);
            }
            return true;
        }

        //Just for drawing; TileOffset being an adjustment position for the light engine the player is using, and TileCustomPosition being a method to get the draw position of any tile given an offset
        public static Vector2 TileOffset => Lighting.LegacyEngine.Mode > 1 ? Vector2.Zero : Vector2.One * 12;
        public static Vector2 TileCustomPosition(int i, int j, Vector2? off = null) => ((new Vector2(i, j) + TileOffset) * 16) - Main.screenPosition - (off ?? new Vector2(0));
    }
}