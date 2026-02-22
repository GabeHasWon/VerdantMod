using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;
using Verdant.Tiles.Verdant.Decor.Terrariums;

namespace Verdant.Items.Verdant.Blocks.Terrariums;

[Sacrifice(1)]
public class JellydewAquariumItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 48, 32, ModContent.TileType<JellydewAquarium>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<AquariumItem>(), 1), (ModContent.ItemType<JellydewItem>(), 1));
}
