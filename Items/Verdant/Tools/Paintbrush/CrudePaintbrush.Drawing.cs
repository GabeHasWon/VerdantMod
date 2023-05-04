using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.UI.Chat;
using Verdant.World;

namespace Verdant.Items.Verdant.Tools.Paintbrush;

public partial class CrudePaintbrush : ApotheoticItem
{
    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        if (_storedItemID == -1)
            return;

        Item item = ContentSamples.ItemsByType[_storedItemID];
        Main.DrawItemIcon(spriteBatch, item, position + new Vector2(36 * scale), Color.White, 24f * scale);

        string count = (GetTileAmmo(Main.LocalPlayer) + 1).ToString();
        var pos = position + (new Vector2(-6, 36) * scale);
        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, count, pos, Color.White, 0f, Vector2.Zero, new Vector2(1f) * scale);
    }

    public void GetLayerDrawing(PlayerDrawSet info)
    {
        if (mode == PlacementMode.Line)
        {
            if (_locations.Count == 1)
                DrawIndicatorLine(info);
        }
        else if (mode == PlacementMode.Oval)
        {
            if (_locations.Count == 1)
                GenHelper.Ellipse((x, y) => AddTileIndicator(info, new(x, y), Color.White), _locations.First(), Main.MouseWorld.ToTileCoordinates());
        }
        else if (mode == PlacementMode.Rectangle)
        {
            if (_locations.Count == 1)
            {
                Point first = _locations.First();
                Point last = Main.MouseWorld.ToTileCoordinates();
                Point topLeft = new(Math.Min(first.X, last.X), Math.Min(first.Y, last.Y));
                Point bottomRight = new(Math.Max(first.X, last.X), Math.Max(first.Y, last.Y));

                int width = bottomRight.X - topLeft.X;
                int height = bottomRight.Y - topLeft.Y;

                for (int x = 0; x < width + 1; ++x)
                {
                    AddTileIndicator(info, new(topLeft.X + x, topLeft.Y), Color.White);
                    AddTileIndicator(info, new(topLeft.X + x, bottomRight.Y), Color.White);
                }

                for (int y = 1; y < height; ++y)
                {
                    AddTileIndicator(info, new(topLeft.X, topLeft.Y + y), Color.White);
                    AddTileIndicator(info, new(bottomRight.X, topLeft.Y + y), Color.White);
                }
            }
        }
    }

    private static int RecursiveFill(PlayerDrawSet info, Point originalPos, int x, int y, ref int repeats, int maxRepeats, List<Point> points)
    {
        if (Main.tile[x, y].HasTile || !WorldGen.InWorld(x, y, 4) || repeats > maxRepeats || points.Contains(new Point(x, y)))
            return repeats;

        AddTileIndicator(info, new Point(x, y), Color.White);
        points.Add(new Point(x, y));
        repeats++;

        if (x >= originalPos.X)
            RecursiveFill(info, originalPos, x + 1, y, ref repeats, maxRepeats, points);

        if (x <= originalPos.X)
            RecursiveFill(info, originalPos, x - 1, y, ref repeats, maxRepeats, points);

        if (y >= originalPos.Y)
            RecursiveFill(info, originalPos, x, y + 1, ref repeats, maxRepeats, points);

        if (y <= originalPos.Y)
            RecursiveFill(info, originalPos, x, y - 1, ref repeats, maxRepeats, points);

        return repeats;
    }

    public static int RecursiveFill(PlayerDrawSet info, Point point, int repeats, int maxRepeats)
    {
        List<Point> points = new List<Point>();
        return RecursiveFill(info, point, point.X, point.Y, ref repeats, maxRepeats, points);
    }

    private void DrawIndicatorLine(PlayerDrawSet info)
    {
        var mouse = Main.MouseWorld.ToTileCoordinates().ToVector2();
        int repeats = (int)Vector2.Distance(_locations.First().ToVector2(), mouse);
        int count = GetTileAmmo(info.drawPlayer);
        Point last = new();

        for (int i = 0; i <= repeats; ++i)
        {
            Color c = i > count - 1 ? Color.Red : Color.White;
            Point placePos = Vector2.Lerp(_locations.First().ToVector2(), mouse, repeats == 0 ? 0 : i / (float)repeats).ToPoint();

            if (placePos == last)
                continue;

            AddTileIndicator(info, placePos, c);
            last = placePos;
        }
    }

    private static void AddTileIndicator(PlayerDrawSet info, Point placePos, Color color)
    {
        const int Offset = 1600;
        int firstTileX = (int)((Main.screenPosition.X - Offset) / 16f - 1f);
        int lastTileX = (int)((Main.screenPosition.X + Main.screenWidth + Offset) / 16f) + 2;
        int firstTileY = (int)((Main.screenPosition.Y - Offset) / 16f - 1f);
        int lastTileY = (int)((Main.screenPosition.Y + Main.screenHeight + Offset) / 16f) + 5;
        Texture2D tex = TextureAssets.Extra[2].Value;

        if (placePos.X < lastTileX && placePos.X > firstTileX && placePos.Y < lastTileY && placePos.Y > firstTileY)
            info.DrawDataCache.Add(new DrawData(tex, placePos.ToWorldCoordinates(0, 0) - Main.screenPosition, new Rectangle(0, 0, 16, 16), color * 0.5f));
    }
}
