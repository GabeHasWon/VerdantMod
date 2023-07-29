using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.PestControl;

public class ThornBlock : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<ThornTile>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<ThornWallItem>(), 4));
        QuickItem.AddRecipe(ModContent.ItemType<ThornWallItem>(), TileID.WorkBenches, 4, (Type, 1));
    }
}
