using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood;

[Sacrifice(1)]
public class LushBedItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushBed>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<VerdantWoodBlock>(), 15), (ItemID.Silk, 5));
}