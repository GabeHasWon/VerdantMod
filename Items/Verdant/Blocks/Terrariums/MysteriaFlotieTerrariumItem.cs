using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Critter;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Decor.Terrariums;

namespace Verdant.Items.Verdant.Blocks.Terrariums;

public class MysteriaFlotieTerrariumItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 48, 48, ModContent.TileType<MysteriaFlotieTerrarium>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, -1, 1, (ItemID.Terrarium, 1), (ModContent.ItemType<LushLeaf>(), 12), (ModContent.ItemType<LushSoilBlock>(), 10),
            (ModContent.ItemType<MysteriaWood>(), 6), (ModContent.ItemType<MysteriaFlotieItem>(), 1), (ModContent.ItemType<MysteriaFluffItem>(), 8));
    }
}
