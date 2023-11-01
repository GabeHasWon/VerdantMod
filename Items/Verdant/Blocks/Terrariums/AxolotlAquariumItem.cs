using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter.Fish;
using Verdant.Tiles.Verdant.Decor.Terrariums;

namespace Verdant.Items.Verdant.Blocks.Terrariums;

[Sacrifice(1)]
public class AxolotlAquariumItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 48, 32, ModContent.TileType<AxolotlAquarium>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<AquariumItem>(), 1), (ModContent.ItemType<AxolotlItem>(), 1));
}
