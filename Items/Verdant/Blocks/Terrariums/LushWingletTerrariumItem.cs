using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Critter;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Decor.Terrariums;

namespace Verdant.Items.Verdant.Blocks.Terrariums;

public class LushWingletTerrariumItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 48, 52, ModContent.TileType<LushWingletTerrarium>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, -1, 1, (ItemID.Terrarium, 1), (ModContent.ItemType<LushLeaf>(), 10), (ModContent.ItemType<LushSoilBlock>(), 8), 
            (ModContent.ItemType<VerdantWoodBlock>(), 6), (ModContent.ItemType<LushWingletItem>(), 1), (ModContent.ItemType<WaterPlantItem>(), 1));
    }
}
