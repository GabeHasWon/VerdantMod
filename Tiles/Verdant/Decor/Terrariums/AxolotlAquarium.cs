using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using Verdant.NPCs.Passive.Fish;
using System;
using Terraria.Audio;

namespace Verdant.Tiles.Verdant.Decor.Terrariums;

public class AxolotlAquarium : Aquarium
{
    public override string Texture => ModContent.GetModTile(ModContent.TileType<Aquarium>()).Texture;

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (Main.rand.NextBool(1000))
            SoundEngine.PlaySound(new SoundStyle("Verdant/Sounds/AxolotlBoop") with { Pitch = 0.85f, PitchVariance = 0.15f }, new Vector2(i, j) * 16);
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];

        if (tile.TileFrameX == 72 && tile.TileFrameY == 36)
        {
            const int Dist = 50;

            Main.instance.LoadNPC(ModContent.NPCType<Axolotl>());
            Texture2D tex = TextureAssets.Npc[ModContent.NPCType<Axolotl>()].Value;
            int xBase = (int)((Main.GameUpdateCount + (i + j * 6)) * 0.35f % Dist);
            int xOffset = xBase > Dist / 2 ? xBase : Dist / 2 - (xBase - Dist / 2);
            Vector2 off = new(8 + xOffset, 16 + MathF.Floor(MathF.Sin(Main.GameUpdateCount * 0.02f) * 4));
            SpriteEffects effect = xBase > Dist / 2 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(tex, TileHelper.TileCustomPosition(i, j, off), new Rectangle(0, 22 * Main.tileFrame[Type], 42, 20), Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, effect, 0);
        }
    }
}