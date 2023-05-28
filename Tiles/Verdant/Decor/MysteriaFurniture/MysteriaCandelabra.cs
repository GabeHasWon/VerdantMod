using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

namespace Verdant.Tiles.Verdant.Decor.MysteriaFurniture;

internal class MysteriaCandelabra : ModTile
{
    public override void SetStaticDefaults() => CandelabraHelper.Defaults(this, new Color(253, 221, 3));
    public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 32, ModContent.ItemType<MysteriaCandelabraItem>());
    public override void HitWire(int i, int j) => CandelabraHelper.WireHit(i, j);

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        Tile tile = Framing.GetTileSafely(i, j);
        if (tile.TileFrameX <= 18 && tile.TileFrameY == 0)
        {
            r = 1f;
            g = 0.75f;
            b = 1f;
        }
    }

    public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
    {
        if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)))
        {
            Tile tile = Main.tile[i, j];
            if (Main.rand.NextBool(40) && tile.TileFrameX <= 18 && tile.TileFrameY == 0)
            {
                int dust = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 + 2), 4, 4, DustID.Torch, 0f, 0f, 100, default, 1f);
                if (!Main.rand.NextBool(3))
                    Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
                Main.dust[dust].velocity.Y = Main.dust[dust].velocity.Y - 1.5f;
            }
        }
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];

        if (tile.TileFrameX > 18 || tile.TileFrameY != 0)
            return;

        Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
        if (Main.drawToScreen)
            zero = Vector2.Zero;

        int width = 32;
        int offsetY = 0;
        int height = 16;
        TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tile.TileFrameX, ref tile.TileFrameY);
        var flameTexture = TextureAssets.Flames[0].Value;

        ulong seed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (uint)i);
        
        for (int c = 0; c < 7; c++)
        {
            float shakeX = Utils.RandomInt(ref seed, -10, 11) * 0.15f;
            float shakeY = Utils.RandomInt(ref seed, -10, 1) * 0.35f;
            Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + zero;
            Main.spriteBatch.Draw(flameTexture, pos + new Vector2(-3, 2), new Rectangle(0, 0, 16, 16), new Color(100, 100, 100, 0), 0f, default, 1f, SpriteEffects.None, 0f);
        }

        if (tile.TileFrameX == 0 && tile.TileFrameY == 0) //extra flame
        {
            for (int c = 0; c < 7; c++)
            {
                float shakeX = Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                float shakeY = Utils.RandomInt(ref seed, -10, 1) * 0.35f;
                Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + zero;
                Main.spriteBatch.Draw(flameTexture, pos + new Vector2(4, -3), new Rectangle(0, 0, 16, 16), new Color(100, 100, 100, 0), 0f, default, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
