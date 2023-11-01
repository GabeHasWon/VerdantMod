using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Critter;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Decor.Terrariums;

namespace Verdant.Items.Verdant.Blocks.Terrariums;

[Sacrifice(1)]
public class MysteriaFlotinyTerrariumItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 32, 48, ModContent.TileType<MysteriaFlotinyTerrarium>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, -1, 1, (ItemID.Terrarium, 1), (ModContent.ItemType<LushLeaf>(), 6), (ModContent.ItemType<LushSoilBlock>(), 8), 
            (ModContent.ItemType<MysteriaWood>(), 3), (ModContent.ItemType<MysteriaFlotinyItem>(), 1), (ModContent.ItemType<MysteriaFluffItem>(), 6));
    }
}
