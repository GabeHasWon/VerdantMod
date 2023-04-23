using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Verdant.World;

namespace Verdant.Items.Verdant.Tools;

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

        switch (_mode)
        {
            case PlacementMode.Line:
                LinePlacement();
                break;
            case PlacementMode.Fill:
                FillPlacement();
                break;
        }
        return true;
    }

    private void FillPlacement() => GenHelper.RecursiveFill(Main.MouseWorld.ToTileCoordinates(), _placeID, 0, 5000);

    private void LinePlacement()
    {
        _locations.Add(Main.MouseWorld.ToTileCoordinates());

        if (_locations.Count == 2)
        {
            GenHelper.GenLine(_locations.First(), _locations.Last(), _placeID);
            _locations.Clear();
        }
        else
            Main.NewText("Place endpoint.");
    }
}
