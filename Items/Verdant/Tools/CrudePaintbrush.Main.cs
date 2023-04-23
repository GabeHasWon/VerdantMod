using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Verdant.Items.Verdant.Tools;

[Sacrifice(1)]
public partial class CrudePaintbrush : ModItem
{
    private enum PlacementMode : byte
    {
        Line,
        Fill,
        Oval,
        Rectangle,
        Count,
    }

    private int _storedItemID = -1;
    private int _placeID = -1;
    private PlacementMode _mode = PlacementMode.Line;
    private List<Point> _locations = new();

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Crude Paintbrush");
        Tooltip.SetDefault("Can rapidly place tiles in various placement styles\nRight click while holding to cycle placement style\n'A crude veneer belies a powerful artefact'");
    }

    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 48;
        Item.useStyle = ItemUseStyleID.Thrust;
        Item.useTurn = true;
        Item.useAnimation = 5;
        Item.useTime = 5;
        Item.autoReuse = false;
        Item.consumable = false;
        Item.maxStack = 99;
    }

    public override bool AltFunctionUse(Player player) => true;
    public override bool CanRightClick() => true;

    public override void SaveData(TagCompound tag)
    {
        tag.Add("placementID", _placeID);
        tag.Add("placementItemIcon", _storedItemID);
    }

    public override void LoadData(TagCompound tag)
    {
        _placeID = tag.GetInt("placementID");
        _storedItemID = tag.GetInt("placementItemIcon");
    }

    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse == 2)
        {
            _mode++;

            if (_mode == PlacementMode.Count)
                _mode = PlacementMode.Line;

            Main.NewText($"Mode switched to {_mode}.");
        }
        return true;
    }

    public override void RightClick(Player player)
    {
        Item.stack++;

        if (player.HeldItem.IsAir)
        {
            Main.NewText("Select a block to start!");
            return;
        }

        int type = player.HeldItem.createTile;

        if (type == -1)
        {
            Main.NewText("Select a valid block to start!");
            return;
        }

        if (Main.tileFrameImportant[type])
        {
            Main.NewText("Select a solid, 1x1 block (like dirt) to start!");
            return;
        }

        _storedItemID = player.HeldItem.type;
        _placeID = type;
        Main.NewText($"Placement type set to {TileID.Search.GetName(_placeID)} ({_placeID}).");
    }
}
