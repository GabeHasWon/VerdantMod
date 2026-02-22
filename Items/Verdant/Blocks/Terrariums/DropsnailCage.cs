using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Decor.Terrariums;

namespace Verdant.Items.Verdant.Blocks.Terrariums;

[Sacrifice(1)]
public class DropsnailCage : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 48, 32, ModContent.TileType<DropsnailTerrarium>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, -1, 1, 
            (ItemID.Terrarium, 1), (ModContent.ItemType<LushLeaf>(), 6), (ModContent.ItemType<LushSoilBlock>(), 10), (Mod.Find<ModItem>("DropsnailItem").Type, 1));
    }
}
