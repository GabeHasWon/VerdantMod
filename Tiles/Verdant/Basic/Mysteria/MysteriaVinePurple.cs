﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Dusts;
using Verdant.Items.Verdant.Blocks.Plants;

namespace Verdant.Tiles.Verdant.Basic.Mysteria;

internal class MysteriaVinePurple : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = false;
        Main.tileCut[Type] = true;
        Main.tileMergeDirt[Type] = false;
        Main.tileBlockLight[Type] = false;
        ItemDrop = 0;
        AddMapEntry(new Color(33, 124, 22));
        DustType = DustID.Grass;
        HitSound = SoundID.Grass;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

    public override void RandomUpdate(int i, int j)
    {
        if (!Main.tile[i, j + 1].HasTile && Main.rand.NextBool(3))
            TileHelper.SyncedPlace(i, j + 1, Type, true);
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (Main.tile[i, j + 1].TileType == Type)
            WorldGen.KillTile(i, j + 1, false, false, true);

        int plr = Player.FindClosest(new Vector2(i, j) * 16, 16, 16);

        if (plr == -1)
            return;

        Player player = Main.player[plr];

        if (player.active && !player.dead && player.GetModPlayer<VerdantPlayer>().expertPlantGuide)
            Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, ModContent.ItemType<VineRopeItem>());
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (!Main.tile[i, j - 1].HasTile)
            WorldGen.KillTile(i, j);
    }

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile t = Framing.GetTileSafely(i, j);
        float sine = (float)Math.Sin((i + j) * MathHelper.ToRadians(20) + Main.GameUpdateCount * 0.04f);

        if (Main.tile[i, j - 1].TileType != Type)
            sine = 0;
        else if (Main.tile[i, j - 2].TileType != Type)
            sine *= 0.33f;
        else if (Main.tile[i, j - 3].TileType != Type)
            sine *= 0.67f;

        spriteBatch.Draw(TextureAssets.Tile[Type].Value, TileHelper.TileCustomPosition(i, j, new Vector2(sine, 0)), new Rectangle(t.TileFrameX, t.TileFrameY, 16, 16), Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        return false;
    }

    public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
    {
        Tile below = Main.tile[i, j];
        below.TileFrameX = 0;
        below.TileFrameY = (short)(Main.rand.Next(4) * 18);

        if (!Main.tile[i, j + 1].HasTile || Main.tile[i, j + 1].TileType != Type)
        {
            below.TileFrameX = 18;
            below.TileFrameY = (short)(Main.rand.Next(2) * 18);
        }
        return false;
    }
}

internal class MysteriaVineOrange : MysteriaVinePurple { }