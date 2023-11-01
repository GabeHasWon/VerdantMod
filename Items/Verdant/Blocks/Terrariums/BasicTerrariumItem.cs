using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Decor.Terrariums;

namespace Verdant.Items.Verdant.Blocks.Terrariums;

[Sacrifice(1)]
public class BasicTerrariumItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 32, 32, ModContent.TileType<BasicTerrarium>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, -1, 1, (ItemID.Terrarium, 1), (ModContent.ItemType<LushLeaf>(), 6), (ModContent.ItemType<LushSoilBlock>(), 4), 
            (ModContent.ItemType<VerdantWoodBlock>(), 1));
    }
}
