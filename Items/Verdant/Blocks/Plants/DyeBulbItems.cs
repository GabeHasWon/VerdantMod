﻿using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Plants;

public class PinkDyeBulb : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 28, 32, ModContent.TileType<Tiles.Verdant.Basic.Plants.DyeBulbs>(), true, 0, ItemRarityID.Blue);
    public override void AddRecipes() => QuickItem.AddRecipe(ItemID.PinkDye, TileID.DyeVat, 1, (ModContent.ItemType<PinkDyeBulb>(), 1));
    public override void ModifyTooltips(List<TooltipLine> tooltips) => tooltips.RemoveAll(x => x.Mod == "Terraria" && x.Name == "Material");
}

public class RedDyeBulb : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 30, 34, ModContent.TileType<Tiles.Verdant.Basic.Plants.DyeBulbs>(), true, 1, ItemRarityID.Blue);
    public override void AddRecipes() => QuickItem.AddRecipe(ItemID.RedDye, TileID.DyeVat, 1, (ModContent.ItemType<RedDyeBulb>(), 1));
    public override void ModifyTooltips(List<TooltipLine> tooltips) => tooltips.RemoveAll(x => x.Mod == "Terraria" && x.Name == "Material");
}
