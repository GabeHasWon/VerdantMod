using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Trees;

internal class PeaceTreeTop : ModTile
{
    public override void Load() => Terraria.On_NPC.SpawnNPC += NPC_SpawnNPC;

    private void NPC_SpawnNPC(Terraria.On_NPC.orig_SpawnNPC orig)
    {
        bool hardMode = Main.hardMode;

        if (PeaceSystem.NearPeace)
            Main.hardMode = false;

        orig();

        if (PeaceSystem.NearPeace)
            Main.hardMode = hardMode;
    }

    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.WoodFurniture, SoundID.Dig, new Color(124, 93, 68), "", true, false);

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
            string type = Main.rand.NextBool(3) ? "LushLeaf" : "PinkPetalFalling";
            Gore.NewGore(new EntitySource_TileUpdate(i, j), pos, Vector2.Zero, Mod.Find<ModGore>(type).Type);
        }
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (fail || noItem)
            return;

        MysteriaTreeTop.ShakeTree(i, j);
    }

    public override IEnumerable<Item> GetItemDrops(int i, int j)
    {
        yield return new Item(ModContent.ItemType<MysteriaAcorn>()) { stack = Main.rand.Next(1, 3) };
        yield return new Item(ModContent.ItemType<MysteriaClump>()) { stack = Main.rand.Next(3, 8) };
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

    private class PeaceSystem : ModSystem
    {
        public static bool NearPeace => ModContent.GetInstance<PeaceSystem>()._peaceCount > 0;

        int _peaceCount;

        public override void ResetNearbyTileEffects() => _peaceCount = 0;
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) => _peaceCount = tileCounts[ModContent.TileType<PeaceTreeTop>()];
    }

    private class PeaceNPC : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (Main.hardMode && PeaceSystem.NearPeace)
            {
                spawnRate = (int)(spawnRate * 0.5f);
                maxSpawns = (int)(maxSpawns * 0.6f);
            }
        }
    }
}