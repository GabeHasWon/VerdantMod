using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;
using Verdant.Tiles.Verdant.Basic.Aquamarine;

namespace Verdant.Items.Verdant.Blocks.Aquamarine;

public class BackslateTileItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<BackslateTile>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<BackslateWallItem>(), 4));
        QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<BackslateBubblingWallItem>(), 3));

        QuickItem.AddRecipe(ModContent.ItemType<BackslateWallItem>(), -1, 4, (Type, 1));
        QuickItem.AddRecipe(ModContent.ItemType<BackslateBubblingWallItem>(), -1, 3, (Type, 1));
    }
}
