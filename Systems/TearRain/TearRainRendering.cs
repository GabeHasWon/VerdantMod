using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Verdant.Dusts;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Systems.TearRain;

[Autoload(true, Side = ModSide.Client)]
internal class TearRainRendering : ModSystem
{
    public struct Rain()
    {
        public Vector2 Velocity = Vector2.Zero;
        public Vector2 Position = Vector2.Zero;
        public Half Opacity = (Half)0f;
        public bool Active = false;
        public byte Frame = 0;
        public float StartY = 0;
    }

    public static List<Rain> Rains = new(4000);
    public static HashSet<int> ValidTiles = [];
    public static Asset<Texture2D> RainTexture = null;

    public override void PostSetupContent()
    {
        ValidTiles = [ModContent.TileType<VerdantLeaves>(), ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>(), ModContent.TileType<LushGrass>()];
        RainTexture = ModContent.Request<Texture2D>("Verdant/Systems/TearRain/Rain");

        for (int i = 0; i < 4000; ++i)
            Rains.Add(new Rain());
    }

    public override void PreUpdateEntities()
    {
        foreach (ref var rain in CollectionsMarshal.AsSpan(Rains))
        {
            if (!rain.Active)
                continue;

            for (int i = 0; i < 2; ++i)
            {
                rain.Velocity.Y = MathHelper.Lerp(rain.Velocity.Y, 8, 0.04f);
                rain.Position += rain.Velocity;

                if (BlockedAtOrLiquid(rain.Position.ToTileCoordinates16()))
                {
                    rain.Active = false;

                    Dust.NewDustPerfect(rain.Position, ModContent.DustType<VerdantWaterSplash>(), new Vector2(Main.rand.NextFloatDirection(), -2));
                }
            }
        }

        if (!TearRainSystem.Raining)
            return;

        const int ScreenOffset = 20;

        Vector2 screenPos = Main.screenPosition;
        int left = (int)(screenPos.X / 16f) - ScreenOffset;
        int right = (int)((screenPos.X + Main.screenWidth) / 16f) + ScreenOffset;
        int top = Math.Min((int)(screenPos.Y / 16f) - ScreenOffset, (int)Main.worldSurface);
        int bottom = (int)((screenPos.Y + Main.screenHeight) / 16f) + ScreenOffset;

        int chance = (int)MathHelper.Lerp(400, 16, TearRainSystem.RainStrength);
        byte frameRange = (byte)(TearRainSystem.RainStrength > 0.6f ? 6 : 3);

        for (int i = left; i <= right; i++)
        {
            for (int j = top; j <= bottom; j++)
            {
                Tile tile = Main.tile[i, j];

                if (!tile.HasTile || !TearRainSystem.CanRain(j) || !ValidTiles.Contains(tile.TileType) || !Main.rand.NextBool(chance) || BlockedAt(new(i, j + 1)))
                    continue;

                CreateRain(i, j, frameRange);
            }
        }
    }

    internal static bool CreateRain(int i, int j, byte frameRange)
    {
        ref Rain rain = ref TryGetFirstRain(out bool success);

        if (!success)
            return false;

        rain.Active = true;
        rain.Position = new Vector2(i, j + 1).ToWorldCoordinates(Main.rand.NextFloat(2, 14), 2);
        rain.Velocity = new Vector2(0, Main.rand.NextFloat(0.25f, 2));
        rain.Frame = (byte)Main.rand.Next(frameRange);
        rain.Opacity = (Half)Main.rand.NextFloat(0.3f, 1f);
        rain.StartY = rain.Position.Y;
        return true;
    }

    private static ref Rain TryGetFirstRain(out bool success)
    {
        foreach (ref Rain r in CollectionsMarshal.AsSpan(Rains))
        {
            if (!r.Active)
            {
                success = true;
                return ref r;
            }
        }

        success = false;
        return ref Unsafe.NullRef<Rain>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool BlockedAt(Point16 pos)
    {
        Tile tile = Main.tile[pos];
        return tile.HasTile && Main.tileSolid[tile.TileType];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool BlockedAtOrLiquid(Point16 pos)
    {
        Tile tile = Main.tile[pos];
        return (tile.HasTile && Main.tileSolid[tile.TileType]) || Main.tile[pos.X, pos.Y - 1].LiquidAmount > 0;
    }

    public override void PostDrawTiles()
    {
        Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, Main.Rasterizer);

        foreach (Rain rain in CollectionsMarshal.AsSpan(Rains))
        {
            if (!rain.Active)
                continue;

            int frameY = 36 * (int)(Main.GameUpdateCount * 0.15f + rain.StartY * 0.75f % 4);
            Rectangle frame = rain.Frame < 3 ? new(8 * rain.Frame, frameY, 6, 18) : new(24 + 12 * (rain.Frame - 3), frameY, 10, 34);
            Color color = Lighting.GetColor(rain.Position.ToTileCoordinates()) with { A = (byte)(255 * rain.Opacity) };
            Vector2 scale = new(1, rain.Velocity.Y / 10f + 0.5f);
            Vector2 origin = frame.Size() * new Vector2(0.5f, 1);

            if (rain.Position.Y < rain.StartY + 10)
                color *= (rain.Position.Y - rain.StartY) / 10f;

            Main.spriteBatch.Draw(RainTexture.Value, rain.Position - Main.screenPosition, frame, color, 0f, origin, scale, SpriteEffects.None, 0);
        }

        Main.spriteBatch.End();
    }
}
