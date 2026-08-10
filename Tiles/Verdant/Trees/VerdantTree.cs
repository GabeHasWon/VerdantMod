using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Items.Verdant.Food;
using Verdant.Items.Verdant.Materials;
using Verdant.NPCs.Passive.Floties;
using Verdant.NPCs.Passive.Snails;

namespace Verdant.Tiles.Verdant.Trees;

public class VerdantTree : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.t_BorealWood, SoundID.Dig, new Color(142, 62, 32), false, false, false, false);
        Main.tileFrameImportant[Type] = true;
        Main.tileAxe[Type] = true;

        RegisterItemDrop(ModContent.ItemType<VerdantWoodBlock>());

        TileID.Sets.IsATreeTrunk[Type] = true;
        TileID.Sets.DrawTileInSolidLayer[Type] = true;
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
    public static bool Spawn(int i, int j, int type = -1, UnifiedRandom r = null, int minSize = 5, int maxSize = 18, bool leaves = false, int leavesType = -1, bool sapling = false)
    {
        if (type == -1) 
            type = ModContent.TileType<VerdantTree>(); //Sets default types

        if (leavesType == -1) 
            leavesType = GoreID.TreeLeaf_Jungle;

        r ??= Main.rand;

        if (sapling)
        {
            WorldGen.KillTile(i, j, false, false, true);
            WorldGen.KillTile(i, j - 1, false, false, true);

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendTileSquare(-1, i, j, 2, 1, TileChangeType.None);
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

        bool hasGround = (TileHelper.SolidTopTile(i + 2, j + 1) || TileHelper.SolidTile(i + 2, j + 1)) && !Framing.GetTileSafely(i + 2, j).HasTile;

        if (hasGround) 
            return false;

        for (int k = 0; k < height; ++k)
        {
            WorldGen.PlaceTile(i, j - k, type, true);
            Framing.GetTileSafely(i, j - k).TileFrameX = 90;
            Framing.GetTileSafely(i, j - k).TileFrameY = (short)(r.Next(3) * 18);

            if (k == height - 1)
            {
                if (r.NextBool(18)) 
                    Framing.GetTileSafely(i, j - k).TileFrameX = 180;
                else 
                    Framing.GetTileSafely(i, j - k).TileFrameX = 198;
            }
            else if (r.NextBool(4) && k > 2)
            {
                int side = r.Next(2);
                if (side == 0 && !Framing.GetTileSafely(i - 1, j - k).HasTile)
                {
                    WorldGen.PlaceTile(i - 1, j - k, type, true);
                    Framing.GetTileSafely(i, j - k).TileFrameX = 162;
                    Framing.GetTileSafely(i - 1, j - k).TileFrameX = 108;
                    Framing.GetTileSafely(i - 1, j - k).TileFrameY = (short)(r.Next(3) * 18);

                    if (Main.netMode != NetmodeID.SinglePlayer)
                        NetMessage.SendTileSquare(-1, i - 1, j - k, TileChangeType.None);
                }
                else if (side == 1 && !Framing.GetTileSafely(i + 1, j - k).HasTile)
                {
                    WorldGen.PlaceTile(i + 1, j - k, type, true);
                    Framing.GetTileSafely(i, j - k).TileFrameX = 144;
                    Framing.GetTileSafely(i + 1, j - k).TileFrameX = 126;
                    Framing.GetTileSafely(i + 1, j - k).TileFrameY = (short)(r.Next(3) * 18);

                    if (Main.netMode != NetmodeID.SinglePlayer)
                        NetMessage.SendTileSquare(-1, i + 1, j - k, TileChangeType.None);
                }
            }

            if (leaves && Main.netMode != NetmodeID.Server)
            {
                if (r.Next(4) <= 1)
                {
                    int rnd = r.Next(2, 5);
                    for (int l = 0; l < rnd; ++l)
                    {
                        Vector2 position = (new Vector2(i, j - k) * 16) + new Vector2(8 + r.Next(-4, 5), 8);
                        Gore.NewGore(Entity.GetSource_NaturalSpawn(), position, new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), leavesType);
                    }
                }
            }

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendTileSquare(-1, i, j - k, TileChangeType.None);
        }
        return true;
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (!Framing.GetTileSafely(i, j + 1).HasTile && !Framing.GetTileSafely(i - 1, j).HasTile && !Framing.GetTileSafely(i + 1, j).HasTile)
            WorldGen.KillTile(i, j, false, false, false);

        if (Framing.GetTileSafely(i, j).TileFrameX == 198 && Main.rand.NextBool(120))
        {
            Vector2 position = (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-56, 56), Main.rand.Next(-44, 44) - 66);
            Gore.NewGore(new EntitySource_TileUpdate(i, j), position, new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("LushLeaf").Type);
        }
    }

    public override IEnumerable<Item> GetItemDrops(int i, int j)
    {
        Tile tile = Framing.GetTileSafely(i, j);

        if (tile.TileFrameX == 198)
        {
            yield return new Item(ModContent.ItemType<VerdantWoodBlock>()) { stack = Main.rand.Next(6, 11) };
            yield return new Item(ItemID.Acorn) { stack = Main.rand.Next(4, 6) };
            yield return new Item(ModContent.ItemType<LushLeaf>()) { stack = Main.rand.Next(7, 15) };
        }

        if (tile.TileFrameX == 108 || tile.TileFrameX == 126)
        {
            yield return new Item(ModContent.ItemType<LushLeaf>()) { stack = Main.rand.Next(3, 7) };
            yield return new Item(ItemID.Acorn) { stack = Main.rand.Next(1, 3) };        
        }

        yield return new Item(ModContent.ItemType<VerdantWoodBlock>()) { stack = Main.rand.Next(1, 3) };
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        Tile t = Framing.GetTileSafely(i, j);

        if (Framing.GetTileSafely(i, j).TileFrameX == 198) //gore stuff
        {
            ClimbToTop(i, j);

            int rnd = Main.rand.Next(8, 14);
            if (fail)
                rnd = Main.rand.Next(2, 6);

            if (Main.netMode != NetmodeID.Server)
                for (int l = 0; l < rnd; ++l)
                {
                    Vector2 position = (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-56, 56), Main.rand.Next(-44, 44) - 66);
                    Gore.NewGore(new EntitySource_TileBreak(i, j), position, new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("LushLeaf").Type);
                }
        }

        if (Framing.GetTileSafely(i, j).TileFrameX == 108 || Framing.GetTileSafely(i, j).TileFrameX == 126)
        {
            int side = Framing.GetTileSafely(i, j).TileFrameX == 108 ? -1 : 1;
            if (fail) //gore
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    int rnd = Main.rand.Next(1, 4);
                    for (int l = 0; l < rnd; ++l)
                    {
                        Vector2 position = (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(40) * side, Main.rand.Next(-10, 10));
                        Gore.NewGore(new EntitySource_TileBreak(i, j), position, new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("LushLeaf").Type);
                    }
                }

                if (Main.rand.NextBool(8))
                {
                    Vector2 position1 = (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(40) * side, Main.rand.Next(-10, 10));
                    Item.NewItem(new EntitySource_TileBreak(i, j), position1, ModContent.ItemType<LushLeaf>(), Main.rand.Next(1, 3));
                }
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
        else if (t.TileFrameX is 90 or 144 or 162 or 180 or 198 or 288 or 306 or 324) //Main tree cut
        {
            int nFrameX = Framing.GetTileSafely(i, j + 1).TileFrameX;

            if (nFrameX == 90) 
                Framing.GetTileSafely(i, j + 1).TileFrameX = 288;

            if (t.TileFrameX == 144) //right branch
            {
                WorldGen.KillTile(i + 1, j, fail);
                Framing.GetTileSafely(i, j + 1).TileFrameX = 306;
            }

            if (nFrameX == 162) 
                Framing.GetTileSafely(i, j + 1).TileFrameX = 324;
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
            else if (Main.tile[i - 1, j].TileType == Type)
                WorldGen.KillTile(i - 1, j);
        }
    }

    private void ClimbToTop(int x, int y) //1.4.4PORT
    {
        while (Main.tile[x, y].HasTile && Main.tile[x, y].TileType == Type)
            y--;
        y++;

        if (Main.tile[x, y].TileFrameX == 198)
        {
            if (!CheckCanTreeShake(x, y))
                return;

            WeightedRandom<int> random = new(Main.rand);

            random.Add(0, 1);
            random.Add(1, 0.7f);
            random.Add(2, 0.85f);

            int rand = random;
            if (rand == 1)
            {
                int type = Main.rand.NextBool() ? ModContent.ItemType<Dropberry>() : ModContent.ItemType<BowlFruit>();
                Vector2 position = (new Vector2(x, y) * 16) + new Vector2(Main.rand.Next(-56, 56), Main.rand.Next(-44, 44) - 66);
                Item.NewItem(new EntitySource_ShakeTree(x, y), position, type, Main.rand.Next(1, 4));
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
                    NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, npcType);
            }

            if (Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 20; ++i)
                {
                    Vector2 position = (new Vector2(x, y) * 16) + new Vector2(Main.rand.Next(-56, 56), Main.rand.Next(-44, 44) - 66);
                    Gore.NewGore(new EntitySource_ShakeTree(x, y), position, new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("LushLeaf").Type);
                }
            }
        }
    }

    public static bool CheckCanTreeShake(int x, int y)
    {
        ref int numTreeShakes = ref GetNumTreeShakes(null);
        int[] treeShakeX = GetTreeShakeX(null);
        int[] treeShakeY = GetTreeShakeY(null);

        for (int k = 0; k < numTreeShakes; k++)
            if (treeShakeX[k] == x && treeShakeY[k] == y)
                return false;

        if (numTreeShakes > treeShakeX.Length)
            return false;

        treeShakeX[numTreeShakes] = x;
        treeShakeY[numTreeShakes] = y;
        numTreeShakes++;
        return true;
    }

    [UnsafeAccessor(UnsafeAccessorKind.StaticField, Name = "numTreeShakes")]
    public static extern ref int GetNumTreeShakes(WorldGen gen);

    [UnsafeAccessor(UnsafeAccessorKind.StaticField, Name = "treeShakeX")]
    public static extern ref int[] GetTreeShakeX(WorldGen gen);

    [UnsafeAccessor(UnsafeAccessorKind.StaticField, Name = "treeShakeY")]
    public static extern ref int[] GetTreeShakeY(WorldGen gen);

    public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
    {
        Tile tile = Main.tile[i, j];

        if (tile.TileFrameX is 108 or 126 or 198)
            Main.instance.TilesRenderer.AddSpecialPoint(i, j, TileDrawing.TileCounterType.CustomNonSolid);
    }

    public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
    {
        if (!GatherVisualInfo(i, j, out Tile t, out Color col, out Texture2D tex, out int frameSize, out int frameOff, out int frameSizY, out Vector2 offset, out Vector2 pos))
            return;

        DrawFoliage(i, j, spriteBatch, t, col, tex, frameSize, frameOff, frameSizY, offset, pos);
    }

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];

        if (tile.TileFrameX is 108 or 126 or 198)
        {
            Main.instance.TilesRenderer.AddSpecialPoint(i, j, TileDrawing.TileCounterType.CustomSolid);
            return false;
        }

        if (!GatherVisualInfo(i, j, out Tile t, out Color col, out Texture2D tex, out int frameSize, out int frameOff, out int frameSizY, out Vector2 offset, out Vector2 pos))
            return false;

        DrawFoliage(i, j, spriteBatch, t, col, tex, frameSize, frameOff, frameSizY, offset, pos);
        return false;
    }

    private static void DrawFoliage(int i, int j, SpriteBatch spriteBatch, Tile t, Color col, Texture2D tex, int frameSize, int frameOff, int frameSizY, Vector2 offset, Vector2 pos)
    {
        Color color = new(col.R, col.G, col.B, 255);

        if (t.TileFrameX == 108) //Draw branches
        {
            int frame = t.TileFrameY / 18;
            spriteBatch.Draw(tex, pos - TileHelper.TileOffset, new Rectangle(292, 52 * frame + 56, 56, 50), color, 0f, new Vector2(38, 16), 1f, SpriteEffects.None, 0f);
            return;
        }
        else if (t.TileFrameX == 126) //Draw branches
        {
            int frame = t.TileFrameY / 18;
            spriteBatch.Draw(tex, pos - TileHelper.TileOffset, new Rectangle(350, 52 * frame + 56, 56, 50), color, 0f, new Vector2(4, 16), 1f, SpriteEffects.None, 0f);
            return;
        }
        else if (t.TileFrameX == 198) // Draw canopy
        {
            int frame = t.TileFrameY / 18;
            TileSwaySystem.DrawTreeSway(i, j + 1, tex, new Rectangle(98 * frame, Main.hardMode ? 162 : 56, 96, 108), offset - TileHelper.TileOffset, new Vector2(40, 96));
        }
        else
        {
            // Draw trunk
            Rectangle src = new(t.TileFrameX + frameOff, t.TileFrameY, frameSize, frameSizY);
            spriteBatch.Draw(tex, pos, src, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        }

        return;
    }

    private static bool GatherVisualInfo(int i, int j, out Tile t, out Color col, out Texture2D tex, out int frameSize, out int frameOff, out int frameSizY, out Vector2 offset, out Vector2 pos)
    {
        t = Framing.GetTileSafely(i, j);

        col = default;
        tex = default;
        frameSize = frameOff = frameSizY = 0;
        offset = pos = Vector2.Zero;

        if (!TileHelper.GetVisualInfo(i, j, out col, out tex))
            return false;

        float xOff = (float)Math.Sin((j * 19) * 0.04f) * 1.2f;

        if (xOff == 1 && (j / 4f) == 0)
            xOff = 0;

        frameSize = 16;
        frameOff = 0;
        frameSizY = 16;
        if (t.TileFrameX == 108 || t.TileFrameX < 36 || t.TileFrameX == 216 || t.TileFrameX == 270)
            frameSize = 18;

        if (t.TileFrameX == 126 || t.TileFrameX == 52 || t.TileFrameX == 72 || t.TileFrameX == 232 || t.TileFrameX == 252)
        {
            frameSize = 18;
            frameOff = -2;
        }

        if (t.TileFrameX < 90 || t.TileFrameX == 216 || t.TileFrameX == 234 || t.TileFrameX == 252 || t.TileFrameX == 270)
            frameSizY = 18;

        offset = new((xOff * 2) - (frameOff / 2), 0);
        pos = TileHelper.TileCustomPosition(i, j, -offset);
        return true;
    }
}
