using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Dusts;
using Verdant.Items.Verdant.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Puff;

internal class PuffVine : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = false;
        Main.tileCut[Type] = true;
        Main.tileMergeDirt[Type] = false;
        Main.tileBlockLight[Type] = false;
        ItemDrop = 0;
        AddMapEntry(new Color(24, 135, 28));
        DustType = DustID.Grass;
        HitSound = SoundID.Grass;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

    public override void RandomUpdate(int i, int j)
    {
        if (!Main.tile[i, j + 1].HasTile && Main.rand.NextBool(3))
            TileHelper.SyncedPlace(i, j + 1, Type, true);
    }

    public override bool Drop(int i, int j)
    {
        int plr = Player.FindClosest(new Vector2(i, j) * 16, 16, 16);

        if (plr == -1)
            return false;

        Player player = Main.player[plr];

        if (player.active && !player.dead && player.GetModPlayer<VerdantPlayer>().expertPlantGuide)
            Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, ModContent.ItemType<VineRopeItem>());
        return false;
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (Main.tile[i, j + 1].TileType == Type)
            WorldGen.KillTile(i, j + 1, false, false, true);
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (!Main.tile[i, j - 1].HasTile)
            WorldGen.KillTile(i, j);

        if (Main.rand.NextBool(480))
        {
            int dust = Dust.NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<PuffDust>(), Scale: Main.rand.NextFloat(0.5f, 0.9f));
            Main.dust[dust].rotation = Main.rand.NextFloat(MathHelper.Pi);
        }
    }

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile t = Framing.GetTileSafely(i, j);

        float sine = (float)Math.Sin((i + j) * MathHelper.ToRadians(20) + Main.GameUpdateCount * 0.02f) * 1f;
        spriteBatch.Draw(TextureAssets.Tile[Type].Value, TileHelper.TileCustomPosition(i, j, new Vector2(sine, 0)), new Rectangle(t.TileFrameX, t.TileFrameY, 16, 16), Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        return false;
    }

    public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
    {
        Tile below = Main.tile[i, j];
        below.TileFrameX = 0;

        if (below.HasTile && below.TileType != Type)
            below.TileFrameX = 0;

        below.TileFrameY = (short)(Main.rand.Next(3) * 18);
        return false;
    }
}