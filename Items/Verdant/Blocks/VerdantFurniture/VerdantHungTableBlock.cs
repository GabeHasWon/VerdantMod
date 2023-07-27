﻿using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    [Sacrifice(1)]
    public class VerdantHungTableBlock_Red : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 42, 26, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantHungTable_Red>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<RedPetal>(), 12), (ModContent.ItemType<VerdantStrongVineMaterial>(), 8), (ModContent.ItemType<Lightbulb>(), 1));
    }

    [Sacrifice(1)]
    public class VerdantHungTableBlock_RedLightless : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 42, 26, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantHungTable_RedLightless>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<RedPetal>(), 14), (ModContent.ItemType<VerdantStrongVineMaterial>(), 8));
    }

    [Sacrifice(1)]
    public class VerdantHungTableBlock_Pink : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 42, 26, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantHungTable_Pink>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<PinkPetal>(), 12), (ModContent.ItemType<VerdantStrongVineMaterial>(), 8), (ModContent.ItemType<Lightbulb>(), 1));
    }

    [Sacrifice(1)]
    public class VerdantHungTableBlock_PinkLightless : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 42, 26, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantHungTable_PinkLightless>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<PinkPetal>(), 14), (ModContent.ItemType<VerdantStrongVineMaterial>(), 8));
    }
}