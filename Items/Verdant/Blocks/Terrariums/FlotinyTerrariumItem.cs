using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Items.Verdant.Critter;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Decor.Terrariums;

namespace Verdant.Items.Verdant.Blocks.Terrariums;

public class FlotinyTerrariumItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 32, 48, ModContent.TileType<FlotinyTerrarium>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, -1, 1, (ItemID.Terrarium, 1), (ModContent.ItemType<LushLeaf>(), 10), (ModContent.ItemType<LushSoilBlock>(), 8), 
            (ModContent.ItemType<VerdantWoodBlock>(), 3), (ModContent.ItemType<FlotinyItem>(), 1));
    }
}
