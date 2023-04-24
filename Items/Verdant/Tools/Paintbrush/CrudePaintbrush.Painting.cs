using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.World;

namespace Verdant.Items.Verdant.Tools.Paintbrush;

public partial class CrudePaintbrush : ModItem
{
    public override bool? UseItem(Player player)
    {
        if (player.altFunctionUse == 2)
            return true;

        if (_placeID == -1)
        {
            Main.NewText("Select an item to start!");
            return true;
        }

        if (GetTileAmmo(player) == -1)
        {
            Main.NewText($"You don't have enough [i:{GetTileWand()}]!");
            return true;
        }

        switch (mode)
        {
            case PlacementMode.Line:
                LinePlacement(player);
                break;
            case PlacementMode.Fill:
                FillPlacement(player);
                break;
            case PlacementMode.Oval:
                OvalPlacement(player);
                break;
        }
        return true;
    }

    private void OvalPlacement(Player player)
    {
        _locations.Add(Main.MouseWorld.ToTileCoordinates());
    }

    private void FillPlacement(Player player)
    {
        _lastChanges.Clear();

        int count = GenHelper.RecursiveFillGetPoints(Main.MouseWorld.ToTileCoordinates(), _placeID, 0, GetTileAmmo(player), ref _lastChanges);
        ConsumeTileWand(count, player);
    }

    private void LinePlacement(Player player)
    {
        _locations.Add(Main.MouseWorld.ToTileCoordinates());

        if (_locations.Count == 2)
        {
            int count = GenHelper.GenLine(_locations.First(), _locations.Last(), _placeID, GetTileAmmo(player));
            ConsumeTileWand(count, player);
            _locations.Clear();
        }
    }

    public void ConsumeTileWand(int count, Player player)
    {
        for (int i = 0; i < player.inventory.Length; ++i)
        {
            Item item = player.inventory[i];

            if (!item.IsAir && item.type == GetTileWand())
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

    public int GetTileWand()
    {
        Item item = ContentSamples.ItemsByType[_storedItemID];
        return item.tileWand > ItemID.None ? item.tileWand : item.type;
    }

    public int GetTileAmmo(Player player) => player.CountItem(GetTileWand(), int.MaxValue) - 1;
}
