using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;

namespace Verdant.Tiles.Verdant.Basic.Plants;

internal class LightbulbVine : ModTile, IFlowerTile
{
    private Asset<Texture2D> glowTex;

    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = false;
        Main.tileCut[Type] = true;
        Main.tileMergeDirt[Type] = false;
        Main.tileBlockLight[Type] = false;
        Main.tileLighted[Type] = true;

        ItemDrop = 0;
        DustType = DustID.Grass;
        HitSound = SoundID.Grass;

        AddMapEntry(new Color(24, 135, 28));
        glowTex = ModContent.Request<Texture2D>(Texture + "_Glow");
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) => (r, g, b) = (0.44f, 0.17f, 0.28f);

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

    public override void RandomUpdate(int i, int j)
    {
        if (!Main.tile[i, j + 1].HasTile && Main.rand.NextBool(8))
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
    }

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile t = Framing.GetTileSafely(i, j);
        float sine = (float)Math.Sin((i + j) * MathHelper.ToRadians(20) + Main.GameUpdateCount * 0.08f) * 1.4f;

        if (Main.tile[i, j - 1].TileType != Type)
            sine = 0;
        else if (Main.tile[i, j - 2].TileType != Type)
            sine *= 0.33f;
        else if (Main.tile[i, j - 3].TileType != Type)
            sine *= 0.67f;

        spriteBatch.Draw(TextureAssets.Tile[Type].Value, TileHelper.TileCustomPosition(i, j, new Vector2(sine, 0)), new Rectangle(t.TileFrameX, t.TileFrameY, 16, 16), Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        spriteBatch.Draw(glowTex.Value, TileHelper.TileCustomPosition(i, j, new Vector2(sine, 0)), new Rectangle(t.TileFrameX, t.TileFrameY, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        return false;
    }

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(8) };

    public bool IsFlower(int i, int j)
    {
        int frameX = Main.tile[i, j].TileFrameX / 18;
        int frameY = Main.tile[i, j].TileFrameY / 18;

        (int, int)[] pairs = new (int, int)[] { (12, 0), (9, 1), (12, 1), (1, 2), (2, 2), (9, 2), (12, 2), (2, 3), (7, 3), (9, 3), (10, 3), (0, 4), (1, 4), (5, 4), (7, 4) };
        return pairs.Any(x => x.Item1 == frameX && x.Item2 == frameY);
    }

    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}