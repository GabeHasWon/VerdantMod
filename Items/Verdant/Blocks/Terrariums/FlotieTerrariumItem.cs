using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Items.Verdant.Critter;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Decor.Terrariums;

namespace Verdant.Items.Verdant.Blocks.Terrariums;

[Sacrifice(1)]
public class FlotieTerrariumItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 48, 48, ModContent.TileType<FlotieTerrarium>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, -1, 1, (ItemID.Terrarium, 1), (ModContent.ItemType<LushLeaf>(), 18), (ModContent.ItemType<LushSoilBlock>(), 10), 
            (ModContent.ItemType<VerdantWoodBlock>(), 6), (ModContent.ItemType<FlotieItem>(), 1));
    }
}
