using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Players;

internal class MudsquidPlayer : ModPlayer
{
    private static int[] MudTypes => new int[] { TileID.Mud, TileID.JungleGrass, TileID.MushroomGrass, ModContent.TileType<LushSoil>() };

    public bool IsSquid => squidActive && SolidCollisionTyped(Player.position, Player.width, Player.height, MudTypes);

    public bool hasSquid = false;
    public bool squidActive = false;

    private float squidAlpha = 1f;

    public override void ResetEffects()
    {
        hasSquid = false;

        if (squidActive)
        {
            if (SolidCollisionTyped(Player.position, Player.width, Player.height, MudTypes))
                Player.gravity = 0;
            Player.noFallDmg = true;
        }

        if (IsSquid)
        {
            if (squidAlpha >= 0.00001f)
                squidAlpha = MathHelper.Lerp(squidAlpha, 0f, 0.33f);
            else
                squidAlpha = 0;
        }
        else
        {
            if (squidAlpha <= 0.999999f)
                squidAlpha = MathHelper.Lerp(squidAlpha, 1f, 0.33f);
            else
                squidAlpha = 1;
        }
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (hasSquid && VerdantMod.SquidHotkey.JustPressed)
        {
            squidActive = !squidActive;
            SetSolids(!squidActive);
        }
    }

    public override void PreUpdateMovement()
    {
        if (IsSquid)
        {
            const float MoveSpeed = 0.85f;
            const float MaxSpeed = 14;

            if (Player.controlDown)
                Player.velocity.Y += MoveSpeed;

            if (Player.controlUp || Player.controlJump)
                Player.velocity.Y -= MoveSpeed;

            if (Player.controlRight)
                Player.velocity.X += MoveSpeed;

            if (Player.controlLeft)
                Player.velocity.X -= MoveSpeed;

            if (Player.velocity.LengthSquared() > MaxSpeed * MaxSpeed)
                Player.velocity = Player.velocity.SafeNormalize(Vector2.Zero) * MaxSpeed;

            if (!Player.controlDown && !Player.controlUp && !Player.controlLeft && !Player.controlRight && !Player.controlJump)
                Player.velocity *= 0.85f;
        }
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (hasSquid)
            (r, g, b, a) = (squidAlpha, squidAlpha, squidAlpha, squidAlpha);
    }

    private static void SetSolids(bool isValid)
    {
        foreach (var item in MudTypes)
            Main.tileSolid[item] = isValid;
    }

    public static bool SolidCollisionTyped(Vector2 position, int width, int height, params int[] types)
    {
        int clampedX = Utils.Clamp((int)(position.X / 16f) - 1, 0, Main.maxTilesX - 1);
        int clampedMaxX = Utils.Clamp((int)((position.X + width) / 16f) + 2, 0, Main.maxTilesX - 1);
        int clampedY = Utils.Clamp((int)(position.Y / 16f) - 1, 0, Main.maxTilesY - 1);
        int clampedMaxY = Utils.Clamp((int)((position.Y + height) / 16f) + 2, 0, Main.maxTilesY - 1);
        Vector2 checkPosition = Vector2.Zero;

        for (int i = clampedX; i < clampedMaxX; i++)
        {
            for (int j = clampedY; j < clampedMaxY; j++)
            {
                Tile tile = Main.tile[i, j];

                if (!tile.IsActuated && tile.HasTile && types.Contains(tile.TileType))
                {
                    checkPosition.X = i * 16;
                    checkPosition.Y = j * 16;
                    int tileHeight = 16;

                    if (tile.IsHalfBlock)
                    {
                        checkPosition.Y += 8f;
                        tileHeight -= 8;
                    }

                    if (position.X + width > checkPosition.X && position.X < checkPosition.X + 16f && position.Y + height > checkPosition.Y && position.Y < checkPosition.Y + tileHeight)
                        return true;
                }
            }
        }
        return false;
    }
}
