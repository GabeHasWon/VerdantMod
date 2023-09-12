using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using Verdant.NPCs.Passive.Fish;
using System;

namespace Verdant.Tiles.Verdant.Decor.Terrariums;

public class MossCarpAquarium : Aquarium
{
    public override string Texture => ModContent.GetModTile(ModContent.TileType<Aquarium>()).Texture;

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];
        if (tile.TileFrameX == 72 && tile.TileFrameY == 36)
        {
            const int Dist = 60;

            Main.instance.LoadNPC(ModContent.NPCType<MossCarp>());
            Texture2D tex = TextureAssets.Npc[ModContent.NPCType<MossCarp>()].Value;
            int xBase = (int)((Main.GameUpdateCount + (i + j * 6)) * 0.1f % Dist);
            int xOffset = xBase > Dist / 2 ? xBase : Dist / 2 - (xBase - Dist / 2);
            Vector2 off = new(-6 + xOffset, 16 + MathF.Floor(MathF.Sin(Main.GameUpdateCount * 0.02f) * 4));
            SpriteEffects effect = xBase > Dist / 2 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            int frameId = (int)(Math.Round(Main.GameUpdateCount * 0.07f) % 4);
            int frame = 26 * frameId;

            if (frameId == 2)
                frame = 0;
            else if (frameId == 3)
                frame = 26 * 2;

            spriteBatch.Draw(tex, TileHelper.TileCustomPosition(i, j, off), new Rectangle(0, frame, 32, 24), Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, effect, 0);
        }
    }
}