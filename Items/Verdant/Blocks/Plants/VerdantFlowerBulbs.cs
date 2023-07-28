using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.Plants;

public class VerdantFlowerBulb : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 30, 28, ModContent.TileType<Tiles.Verdant.Mounted.Flower_2x2>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<PinkPetal>(), 3), (ModContent.ItemType<RedPetal>(), 3), (ModContent.ItemType<LushLeaf>(), 2));
}

public class LargeFlowerBulb : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 36, 34, ModContent.TileType<Tiles.Verdant.Mounted.Flower_3x3>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<PinkPetal>(), 7), (ModContent.ItemType<RedPetal>(), 7), (ModContent.ItemType<LushLeaf>(), 4));
}

public class LightbulbFlowerBulb : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 30, 28, ModContent.TileType<Tiles.Verdant.Mounted.MountedLightbulb_2x2>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<PinkPetal>(), 4), (ModContent.ItemType<RedPetal>(), 4), 
        (ModContent.ItemType<LushLeaf>(), 2), (ModContent.ItemType<Lightbulb>(), 1));
}