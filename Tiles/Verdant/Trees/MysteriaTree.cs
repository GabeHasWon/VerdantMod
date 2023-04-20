using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Misc;

namespace Verdant.Tiles.Verdant.Trees;

internal class MysteriaTree : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.WoodFurniture, SoundID.Dig, new Color(90, 120, 90), ModContent.ItemType<OvergrownBrickItem>(), "", true, false);

        Main.tileBlendAll[Type] = true;
        Main.tileBrick[Type] = true;
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (fail)
            return;

        void KillIfAlsoTree(int x, int y)
        {
            if (Main.tile[x, y].HasTile && Main.tile[x, y].TileType == Type)
                WorldGen.KillTile(x, y);
        }

        for (int x = -1; x < 2; ++x)
            KillIfAlsoTree(i + x, j - 1);
    }

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];

        int frameX = tile.TileFrameX / 18 * 22;
        int frameY = tile.TileFrameY / 18 * 22;

        Vector2 pos = TileHelper.TileCustomPosition(i, j, new Vector2(2));
        var source = new Rectangle(frameX, frameY, 20, 20);
        spriteBatch.Draw(TextureAssets.Tile[Type].Value, pos, source, Lighting.GetColor(i, j));
        return false;
    }
}

internal class MysteriaTreeTop : ModTile
{
    public override string Texture => base.Texture[..(base.Texture.Length - 3)];

    Asset<Texture2D> _tops;

    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.WoodFurniture, SoundID.Dig, new Color(90, 120, 90), ModContent.ItemType<OvergrownBrickItem>(), "", true, false);

        Main.tileBlendAll[Type] = true;
        Main.tileBrick[Type] = true;
        Main.tileSolid[Type] = false;

        _tops = ModContent.Request<Texture2D>(Texture + "Tops");
    }

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];

        int frameX = tile.TileFrameX / 18 * 22;
        int frameY = tile.TileFrameY / 18 * 22;

        if (frameX >= 132 && frameX <= 176 && frameY == 0)
        {
            Rectangle treeSource = new(0, (frameX - 132) / 22 * 102, 196, 100);
            SpriteEffects effects = i % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            TileSwaySystem.DrawTreeSway(i, j, _tops.Value, treeSource, new Vector2(8, 16), new Vector2(98, 100), effects);
        }

        return false;
    }
}