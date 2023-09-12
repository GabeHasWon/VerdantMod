using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using Verdant.NPCs.Passive.Fish;
using System;

namespace Verdant.Tiles.Verdant.Decor.Terrariums;

public class PoolwormAquarium : Aquarium
{
    public override string Texture => ModContent.GetModTile(ModContent.TileType<Aquarium>()).Texture;

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];
        if (tile.TileFrameX == 72 && tile.TileFrameY == 36)
        {
            const int Dist = 60;

            Main.instance.LoadNPC(ModContent.NPCType<Poolworm>());
            Texture2D tex = TextureAssets.Npc[ModContent.NPCType<Poolworm>()].Value;
            int xBase = (int)(Main.GameUpdateCount * 0.25f % Dist);
            int xOffset = xBase > Dist / 2 ? xBase : Dist / 2 - (xBase - Dist / 2);
            Vector2 off = new(-12 + xOffset, 16 + MathF.Floor(MathF.Sin(Main.GameUpdateCount * 0.02f) * 4));
            SpriteEffects effect = xBase > Dist / 2 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(tex, TileHelper.TileCustomPosition(i, j, off), new Rectangle(0, 0, 22, 12), Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, effect, 0);
        }
    }
}