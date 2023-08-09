using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Systems.RealtimeGeneration;

namespace Verdant;

public static class Extensions
{
    public static Point Add(this Point p, Point other) => new(p.X + other.X, p.Y + other.Y);
    public static Point Mul(this Point p, int mult) => new(p.X * mult, p.Y * mult);

    public static Point TileCoordsBottomCentred(this Player p, Vector2? offset = null)
    {
        Vector2 off = offset ?? Vector2.Zero;
        return new Point((int)((p.Center.X + off.X - 8) / 16f), (int)((p.Bottom.Y + off.Y + 4) / 16f));
    }

    public static Point TileCoordsBottomLeft(this Player p, Vector2? offset = null)
    {
        Vector2 off = offset ?? Vector2.Zero;
        return new Point((int)((p.BottomLeft.X + off.X) / 16f), (int)((p.BottomLeft.Y + off.Y + 4) / 16f));
    }

    public static Point TileCoordsBottomRight(this Player p, Vector2? offset = null)
    {
        Vector2 off = offset ?? Vector2.Zero;
        return new Point((int)((p.BottomRight.X + off.X) / 16f), (int)((p.BottomRight.Y + off.Y + 4) / 16f));
    }

    public static Color GetLightColor(this NPC n) => Lighting.GetColor((int)(n.Center.X / 16f), (int)(n.Center.Y / 16f));

    public static bool ArmourEquipped(this Player player, int type)
    {
        for (int k = 0; k <= 3; k++)
            if (player.armor[k].type == type) return true;
        return false;
    }
    public static bool ArmourEquipped(this Player player, Item item) => player.ArmourEquipped(item.type);

    public static bool ArmourSetEquipped(this Player player, int head, int body, int legs) => (player.armor[0].type == head && player.armor[1].type == body && player.armor[2].type == legs);

    public static bool AccessoryEquipped(this Player player, int type)
    {
        for (int k = 3; k <= 7 + player.extraAccessorySlots; k++)
            if (player.armor[k].type == type) return true;
        return false;
    }
    public static bool AccessoryEquipped(this Player player, Item item) => player.AccessoryEquipped(item.type);

    public static Player Owner(this Projectile p) => Main.player[p.owner];

    public static TileState GetState(this Tile tile, int i, int j, string from)
    {
        TileState tileState = new(
            new Terraria.DataStructures.Point16(i, j),
            tile.HasTile,
            tile.TileType,
            tile.TileFrameX,
            tile.TileFrameY,
            tile.WallType,
            (short)tile.WallFrameX,
            (short)tile.WallFrameY,
            (byte)tile.LiquidType,
            tile.LiquidAmount,
            from,
            tile.Slope
        );

        return tileState;
    }

    public static Vector2 GetRealDrawPosition(this PlayerDrawSet info, Vector2 offset)
    {
        offset += info.Position - Main.screenPosition;

        if (info.drawPlayer.mount is not null && info.drawPlayer.mount.Active)
            offset.Y += info.drawPlayer.mount.HeightBoost;

        offset = offset.ToPoint().ToVector2();
        return new(MathF.Round(offset.X, MidpointRounding.ToNegativeInfinity), MathF.Round(offset.Y, MidpointRounding.AwayFromZero));
    }

    public static int GetBobble(this Player player)
    {
        int playerFrame = player.bodyFrame.Y / player.bodyFrame.Height;
        bool lowFrame = (playerFrame >= 7 && playerFrame <= 9) || (playerFrame >= 14 && playerFrame <= 16);
        return lowFrame ? 0 : -2;
    }
}