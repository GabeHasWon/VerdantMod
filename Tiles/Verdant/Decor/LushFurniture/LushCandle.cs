using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles.Verdant.Decor.LushFurniture
{
    internal class LushCandle : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.WaterDeath = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

            AddMapEntry(new Color(253, 221, 3), CreateMapEntryName());
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail && !noItem)
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 32, ModContent.ItemType<Items.Verdant.Blocks.LushWood.LushCandleItem>());
        }

        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int topY = j - tile.TileFrameY / 18 % 3;
            short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);
            Main.tile[i, topY].TileFrameX += frameAdjustment;
            Wiring.SkipWire(i, topY);
            NetMessage.SendTileSquare(-1, i, topY + 1, 1, TileChangeType.None);
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) => spriteEffects = i % 2 == 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Framing.GetTileSafely(i, j).TileFrameX == 0)
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
                if (Main.rand.NextBool(40) && tile.TileFrameX == 0 && tile.TileFrameY / 18 % 3 == 0)
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
            if (Framing.GetTileSafely(i, j).TileFrameX != 0)
                return;

            SpriteEffects effects = SpriteEffects.None;
            if (i % 2 == 1)
                effects = SpriteEffects.FlipHorizontally;

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
                zero = Vector2.Zero;

            Tile tile = Main.tile[i, j];
            int width = 16;
            int offsetY = 0;
            int height = 16;
            int offsetX = i % 2 == 0 ? 3 : -3;
            TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tile.TileFrameX, ref tile.TileFrameY);
            var flameTexture = TextureAssets.Flames[0].Value;

            ulong seed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (uint)i);
            
            for (int c = 0; c < 7; c++)
            {
                float shakeX = Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                float shakeY = Utils.RandomInt(ref seed, -10, 1) * 0.35f;
                Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + zero;
                Main.spriteBatch.Draw(flameTexture, pos + new Vector2(-offsetX, 2), new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(100, 100, 100, 0), 0f, default, 1f, effects, 0f);
            }
        }
    }
}
