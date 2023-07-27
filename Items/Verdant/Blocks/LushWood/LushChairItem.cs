using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood;

[Sacrifice(1)]
public class LushChairItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushChair>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, Terraria.ID.TileID.WorkBenches, 1, (ModContent.ItemType<VerdantWoodBlock>(), 4));
}