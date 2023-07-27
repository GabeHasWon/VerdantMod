using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Tools.Paintbrush;

[Sacrifice(1)]
public partial class CrudePaintbrush : ApotheoticItem
{
    public enum PlacementMode : byte
    {
        Line,
        Fill,
        Oval,
        Rectangle,
        Count,
    }

    private readonly List<Point> _locations = new();

    public PlacementMode mode = PlacementMode.Line;

    private int _storedItemID = -1;
    private int _storedRefundID = -1;
    private int _placedTileID = -1;
    private List<Point> _lastChanges = new();

    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Crude Paintbrush");
        // Tooltip.SetDefault("Can rapidly place tiles in various placement styles\nRight click while holding to cycle placement style\n'A crude veneer belies a powerful artefact'");
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
        Item.maxStack = 1;
        Item.rare = ItemRarityID.Red;
    }

    public override bool AltFunctionUse(Player player) => true;
    public override bool CanRightClick() => true;

    public override void SaveData(TagCompound tag)
    {
        if (_placedTileID < TileID.Count)
            tag.Add("placementIDInt", _placedTileID);
        else
            tag.Add("placementIDString", ModContent.GetModTile(_placedTileID).FullName);

        tag.Add("placementIDIsInt", _placedTileID < TileID.Count);

        if (_storedItemID < ItemID.Count)
            tag.Add("placementItemIconInt", _storedItemID);
        else
            tag.Add("placementItemIconString", ModContent.GetModItem(_storedItemID).FullName);

        tag.Add("placementItemIconIsInt", _storedItemID < ItemID.Count);
    }

    public override void LoadData(TagCompound tag)
    {
        bool isInt = tag.GetBool("placementIDIsInt");
        if (isInt)
            _placedTileID = tag.GetInt("placementIDInt");
        else
        {
            if (ModContent.TryFind(tag.GetString("placementIDString"), out ModTile item))
                _placedTileID = item.Type;
            else
                _placedTileID = -1;
        }

        bool iconIsInt = tag.GetBool("placementItemIconIsInt");
        if (iconIsInt)
            _storedItemID = tag.GetInt("placementItemIconInt");
        else
        {
            if (ModContent.TryFind(tag.GetString("placementItemIconString"), out ModItem item))
                _storedItemID = item.Type;
            else
                _storedItemID = -1;
        }
    }

    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse == 2)
            CrudePaintbrushUISystem.Toggle();
        return !CrudePaintbrushUISystem.Open;
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
        _placedTileID = type;

        Main.NewText($"Placement type set to [i:{GetTileWand()}] ({TileID.Search.GetName(_placedTileID)})");
    }

    internal void SetMode(PlacementMode index)
    {
        mode = index;
        _locations.Clear();
    }

    internal void Undo(Player player)
    {
        if (_lastChanges.Any())
        {
            foreach (var item in _lastChanges)
                WorldGen.KillTile(item.X, item.Y, false, false, true);

            var first = _lastChanges.First();
            int count = _lastChanges.Count;
            int max = ContentSamples.ItemsByType[_storedRefundID].maxStack;

            while (count > 0)
            {
                player.QuickSpawnItem(new EntitySource_TileBreak(first.X, first.Y), GetTileWand(), Math.Min(count, max));
                count -= max;
            }

            _lastChanges.Clear();
        }
        else
            Main.NewText("No changes to undo.");
    }

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(CrudePaintbrush))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.CrudePaintbrush.", 3, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.CrudePaintbrush.0", 80).
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.CrudePaintbrush.1", 90, 1f)).
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.CrudePaintbrush.2", 100, 1f));
    }
}
