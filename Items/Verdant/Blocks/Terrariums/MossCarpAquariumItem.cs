using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter.Fish;
using Verdant.Tiles.Verdant.Decor.Terrariums;

namespace Verdant.Items.Verdant.Blocks.Terrariums;

public class MossCarpAquariumItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 48, 32, ModContent.TileType<MossCarpAquarium>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<AquariumItem>(), 1), (ModContent.ItemType<MossCarpItem>(), 1));
}
