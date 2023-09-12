using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using Verdant.NPCs.Passive.Fish;
using System;

namespace Verdant.Tiles.Verdant.Decor.Terrariums;

public class BulbboxJellyAquarium : Aquarium
{
    public override string Texture => ModContent.GetModTile(ModContent.TileType<Aquarium>()).Texture;

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];
        if (tile.TileFrameX == 72 && tile.TileFrameY == 36)
        {
            float[] offsets =   new[] { 0f, 18, 22, 26, 26, 26, 26, 10, -10, -16, -20, -20, -20, -20, -6, 0 };
            float[] rotations = new[] { 1,  1f, 1f, 1,  1,  1,  -1,  -1, -1,  -1,  -1,  -1,  -1,  1,  1,  1 };

            Main.instance.LoadNPC(ModContent.NPCType<BulbboxJelly>());
            Texture2D tex = TextureAssets.Npc[ModContent.NPCType<BulbboxJelly>()].Value;
            float offset = (Main.GameUpdateCount * 0.02f) + ((i + j) * MathHelper.PiOver2);
            int index = (int)Math.Ceiling(offset) % offsets.Length;
            Vector2 off = new(MathHelper.Lerp(offsets[index], offsets[index == offsets.Length - 1 ? 0 : index + 1], offset % 1) + 20, 8);
            float rot = rotations[index] == 1 ? -MathHelper.PiOver2 : MathHelper.PiOver2;
            var src = new Rectangle(0, 52, 22, 24);

            spriteBatch.Draw(tex, TileHelper.TileCustomPosition(i, j, new Vector2(MathF.Round(off.X), off.Y)), src, Lighting.GetColor(i, j), rot, new(11, 12), 1f, SpriteEffects.None, 0);
        }
    }
}