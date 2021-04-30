using System;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks
{
    public class VerdantHungTableBlock_Red : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Hanging Lush Table (Red)");
        public override void SetDefaults() => QuickItem.SetBlock(this, 42, 26, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantHungTable_Red>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemType<RedPetal>(), 12), (ItemType<VerdantStrongVineMaterial>(), 8), (ItemType<Lightbulb>(), 1));
    }

    public class VerdantHungTableBlock_RedLightless : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Hanging Lush Table (Red, No Bulb)");
        public override void SetDefaults() => QuickItem.SetBlock(this, 42, 26, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantHungTable_RedLightless>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemType<RedPetal>(), 14), (ItemType<VerdantStrongVineMaterial>(), 8));
    }

    public class VerdantHungTableBlock_Pink : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Hanging Lush Table (Pink)");
        public override void SetDefaults() => QuickItem.SetBlock(this, 42, 26, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantHungTable_Pink>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemType<PinkPetal>(), 12), (ItemType<VerdantStrongVineMaterial>(), 8), (ItemType<Lightbulb>(), 1));
    }

    public class VerdantHungTableBlock_PinkLightless : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Hanging Lush Table (Pink, No Bulb)");
        public override void SetDefaults() => QuickItem.SetBlock(this, 42, 26, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantHungTable_PinkLightless>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemType<PinkPetal>(), 14), (ItemType<VerdantStrongVineMaterial>(), 8));
    }
}