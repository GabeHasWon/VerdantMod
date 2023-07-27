using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

[Sacrifice(1)]
public class MysteriaPlatformItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 10, ModContent.TileType<Tiles.Verdant.Decor.MysteriaFurniture.MysteriaPlatforms>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, -1, 2, (ModContent.ItemType<MysteriaWood>(), 1));
        QuickItem.AddRecipe(ModContent.ItemType<MysteriaWood>(), -1, 1, (Type, 2));
    }
}