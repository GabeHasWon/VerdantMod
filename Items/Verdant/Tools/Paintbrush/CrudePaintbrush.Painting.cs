using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Verdant.World;

namespace Verdant.Items.Verdant.Tools.Paintbrush;

public partial class CrudePaintbrush : ApotheoticItem
{
    public int GetTileIDToPlace 
    {
        get
        {
            Item item = ContentSamples.ItemsByType[_storedItemID];
            return item.tileWand > ItemID.None ? item.tileWand : item.type;
        }
    }

    public override bool? UseItem(Player player)
    {
        if (player.altFunctionUse == 2)
            return true;

        if (_placedTileID == -1)
        {
            Main.NewText(Language.GetTextValue("Mods.Verdant.Items.CrudePaintbrush.Information.NoItemSelected"));
            return true;
        }

        if (GetAvailableBlocks(player) == -1)
        {
            Main.NewText(Language.GetText("Mods.Verdant.Items.CrudePaintbrush.Information.NotEnoughItem").WithFormatArgs(GetTileIDToPlace).Value);
            return true;
        }

        switch (tool)
        {
            case ToolType.Line:
                LinePlacement(player);
                break;
            case ToolType.Fill:
                FillPlacement(player);
                break;
            case ToolType.Oval:
                OvalPlacement(player);
                break;
            case ToolType.Rectangle:
                RectanglePlacement(player);
                break;
        }
        return true;
    }

    private void RectanglePlacement(Player player)
    {
        _locations.Add(Main.MouseWorld.ToTileCoordinates());

        if (_locations.Count > 1)
        {
            _lastChanges.Clear();

            _storedRefundID = GetTileIDToPlace;

            Point first = _locations.First();
            Point last = Main.MouseWorld.ToTileCoordinates();

            Point topLeft = new(Math.Min(first.X, last.X), Math.Min(first.Y, last.Y));
            Point bottomRight = new(Math.Max(first.X, last.X), Math.Max(first.Y, last.Y));

            int width = bottomRight.X - topLeft.X;
            int height = bottomRight.Y - topLeft.Y;
            int count = 0;

            for (int x = 0; x < width + 1; ++x)
            {
                WorldGen.PlaceTile(topLeft.X + x, topLeft.Y, _placedTileID, true);
                WorldGen.PlaceTile(topLeft.X + x, bottomRight.Y, _placedTileID, true);

                _lastChanges.Add(new Point(topLeft.X + x, topLeft.Y));
                _lastChanges.Add(new Point(topLeft.X + x, bottomRight.Y));
                count += 2;
            }

            for (int y = 1; y < height; ++y)
            {
                WorldGen.PlaceTile(topLeft.X, topLeft.Y + y, _placedTileID, true);
                WorldGen.PlaceTile(bottomRight.X, topLeft.Y + y, _placedTileID, true);

                _lastChanges.Add(new Point(topLeft.X, topLeft.Y + y));
                _lastChanges.Add(new Point(bottomRight.X, topLeft.Y + y));
                count += 2;
            }

            ConsumeTileWand(count, player);
            _locations.Clear();
        }
    }

    private void OvalPlacement(Player player)
    {
        _locations.Add(Main.MouseWorld.ToTileCoordinates());

        if (_locations.Count > 1)
        {
            _lastChanges.Clear();
            _storedRefundID = GetTileIDToPlace;

            int count = 0;

            GenHelper.Ellipse((x, y) =>
            {
                WorldGen.PlaceTile(x, y, _placedTileID);
                count++;
            }, _locations.First(), Main.MouseWorld.ToTileCoordinates(), ref _lastChanges);

            ConsumeTileWand(count, player);
            _locations.Clear();
        }
    }

    private void FillPlacement(Player player)
    {
        _locations.Add(default);

        if (_locations.Count == 2)
        {
            _locations.Clear();
            _lastChanges.Clear();
            _storedRefundID = GetTileIDToPlace;

            int count = GenHelper.RecursiveFillGetPoints(Main.MouseWorld.ToTileCoordinates(), _placedTileID, 0, GetAvailableBlocks(player), ref _lastChanges, true);
            ConsumeTileWand(count, player);

            foreach (var item in _lastChanges)
            {
                for (int i = -1; i < 2; ++i)
                {
                    for (int j = -1; j < 2; ++j)
                    {
                        WorldGen.SquareTileFrame(item.X + i, item.Y + j, true);
                        Tile.SmoothSlope(item.X + i, item.Y + j, true);
                    }
                }
            }
        }
        else
            Main.NewText(Language.GetTextValue("Mods.Verdant.Items.CrudePaintbrush.Information.FillConfirm"));
    }

    private void LinePlacement(Player player)
    {
        _locations.Add(Main.MouseWorld.ToTileCoordinates());

        if (_locations.Count == 2)
        {
            _lastChanges.Clear();
            _storedRefundID = GetTileIDToPlace;

            int count = GenHelper.GenLine(_locations.First(), _locations.Last(), _placedTileID, ref _lastChanges, GetAvailableBlocks(player));
            ConsumeTileWand(count, player);
            _locations.Clear();
        }
    }

    public void ConsumeTileWand(int count, Player player)
    {
        for (int i = 0; i < player.inventory.Length; ++i)
        {
            Item item = player.inventory[i];

            if (!item.IsAir && item.type == GetTileIDToPlace && item.consumable)
            {
                if (count <= 0)
                    break;

                int stack = item.stack;

                if (count >= item.stack)
                {
                    item.stack = 1;
                    item.TurnToAir();
                }
                else
                    item.stack -= count;

                count -= stack;
            }
        }
    }

    public int GetAvailableBlocks(Player player)
    {
        int wand = GetTileIDToPlace;
        int count = !ContentSamples.ItemsByType[wand].consumable ? 1000 : player.CountItem(wand, int.MaxValue) - 1;
        return tool == ToolType.Fill ? Math.Min(count, 1000) : count;
    }
}
