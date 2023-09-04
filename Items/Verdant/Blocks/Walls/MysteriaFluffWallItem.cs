using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls;

public class MysteriaFluffWallItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetWall(this, 32, 32, ModContent.WallType<MysteriaFluffWall>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, -1, 4, (ModContent.ItemType<MysteriaFluffItem>(), 1));
        QuickItem.AddRecipe(ModContent.ItemType<MysteriaFluffItem>(), -1, 1, (Type, 4));
    }
}