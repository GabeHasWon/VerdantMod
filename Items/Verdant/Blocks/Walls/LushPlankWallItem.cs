using Terraria.ModLoader;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls;

public class LushPlankWallItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetWall(this, 32, 32, ModContent.WallType<LushPlankWall>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, -1, 4, (ModContent.ItemType<LushWood.LushWoodPlankBlock>(), 1));
        QuickItem.AddRecipe(ModContent.ItemType<LushWood.LushWoodPlankBlock>(), -1, 1, (Type, 4));
    }
}