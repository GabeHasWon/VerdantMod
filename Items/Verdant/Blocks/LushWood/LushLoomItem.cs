using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.LushWood;

[Sacrifice(1)]
public class LushLoomItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 36, 26, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushLoom>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.Sawmill, 1, (ModContent.ItemType<VerdantWoodBlock>(), 10), (ItemID.Cobweb, 5), (ModContent.ItemType<LushLeaf>(), 10));
}