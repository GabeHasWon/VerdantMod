using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks.Plants;

public class PinkDyeBulb : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 28, 32, ModContent.TileType<DyeBulbs>(), true, 0, ItemRarityID.Blue, Item.buyPrice(0, 0, 40));
    public override void AddRecipes() => QuickItem.AddRecipe(ItemID.PinkDye, TileID.DyeVat, 1, (Type, 1));
    public override void ModifyTooltips(List<TooltipLine> tooltips) => tooltips.RemoveAll(x => x.Mod == "Terraria" && x.Name == "Material");
}

public class RedDyeBulb : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 30, 34, ModContent.TileType<DyeBulbs>(), true, 1, ItemRarityID.Blue, Item.buyPrice(0, 0, 40));
    public override void AddRecipes() => QuickItem.AddRecipe(ItemID.RedDye, TileID.DyeVat, 1, (Type, 1));
    public override void ModifyTooltips(List<TooltipLine> tooltips) => tooltips.RemoveAll(x => x.Mod == "Terraria" && x.Name == "Material");
}

public class BlueDyeBulb : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 30, 34, ModContent.TileType<DyeBulbs>(), true, 2, ItemRarityID.Blue, Item.buyPrice(0, 0, 40));
    public override void AddRecipes() => QuickItem.AddRecipe(ItemID.BlueDye, TileID.DyeVat, 1, (Type, 1));
    public override void ModifyTooltips(List<TooltipLine> tooltips) => tooltips.RemoveAll(x => x.Mod == "Terraria" && x.Name == "Material");
}

public class WhiteDyeBulb : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 30, 34, ModContent.TileType<DyeBulbs>(), true, 3, ItemRarityID.Blue, Item.buyPrice(0, 0, 40));
    public override void AddRecipes() => QuickItem.AddRecipe(ItemID.SilverDye, TileID.DyeVat, 1, (Type, 1));
    public override void ModifyTooltips(List<TooltipLine> tooltips) => tooltips.RemoveAll(x => x.Mod == "Terraria" && x.Name == "Material");
}
