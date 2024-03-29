﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Food;
using Verdant.Items.Verdant.Materials;
using Verdant.NPCs.Passive.Floties;

namespace Verdant.Tiles.Verdant.Trees;

internal class MysteriaTreeTop : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.WoodFurniture, SoundID.Dig, new Color(124, 93, 68), true, false);

        Main.tileBrick[Type] = true;
        Main.tileSolid[Type] = false;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
        TileObjectData.newTile.AnchorBottom = new AnchorData(Terraria.Enums.AnchorType.AlternateTile, 1, 0);
        TileObjectData.newTile.AnchorAlternateTiles = new int[] { ModContent.TileType<MysteriaTree>() };
        TileObjectData.addTile(Type);
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (Main.rand.NextBool(80))
        {
            var pos = (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-36, 36), Main.rand.Next(44) - 58);
            string type = Main.rand.NextBool(3) ? "LushLeaf" : "PurplePetalFalling";
            Gore.NewGore(new EntitySource_TileUpdate(i, j), pos, Vector2.Zero, Mod.Find<ModGore>(type).Type);
        }
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (fail || noItem)
            return;

        ShakeTree(i, j);
    }

    public override IEnumerable<Item> GetItemDrops(int i, int j)
    {
        yield return new Item(ModContent.ItemType<MysteriaAcorn>()) { stack = Main.rand.Next(1, 3) };
        yield return new Item(ModContent.ItemType<MysteriaClump>()) { stack = Main.rand.Next(3, 8) };
    }

    internal static void ShakeTree(int x, int y) //1.4.4PORT
    {
        //for (int k = 0; k < WorldGen.numTreeShakes; k++)
        //    if (WorldGen.treeShakeX[k] == x && WorldGen.treeShakeY[k] == y)
        //        return;

        //WorldGen.treeShakeX[WorldGen.numTreeShakes] = x;
        //WorldGen.treeShakeY[WorldGen.numTreeShakes] = y;
        //WorldGen.numTreeShakes++;

        WeightedRandom<int> random = new(Main.rand);

        random.Add(0, 1);
        random.Add(1, 0.7f);
        random.Add(2, 0.85f);

        int rand = random;
        if (rand == 1)
        {
            int type = Main.rand.NextBool() ? ModContent.ItemType<Walnut>() : ModContent.ItemType<Mystuber>();
            Item.NewItem(new EntitySource_ShakeTree(x, y), (new Vector2(x, y) * 16) + new Vector2(Main.rand.Next(-56, 56), Main.rand.Next(-44, 44) - 66), type, Main.rand.Next(1, 4));
        }
        else if (rand == 2)
        {
            int reps = Main.rand.Next(3, 6);

            WeightedRandom<int> npcType = new(Main.rand);
            npcType.Add(ModContent.NPCType<MysteriaFlotie>(), 0.9f);
            npcType.Add(ModContent.NPCType<MysteriaFlotiny>(), 1.2f);

            for (int i = 0; i < reps; ++i)
                NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, npcType);
        }
    }

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];
        int frameX = tile.TileFrameX / 18 * 22;
        Rectangle treeSource = new(0, frameX / 22 % 3 * 102, 196, 100);
        SpriteEffects effects = i % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        TileSwaySystem.DrawTreeSway(i, j, TextureAssets.Tile[Type].Value, treeSource, new Vector2(8, 16), new Vector2(98, 100), effects);
        return false;
    }
}