using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls;

public class MysteriaWoodWallItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Mysteria Wood Wall", "");
    public override void SetDefaults() => QuickItem.SetWall(this, 32, 32, ModContent.WallType<MysteriaWoodWall>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, TileID.WorkBenches, 4, (ModContent.ItemType<MysteriaWood>(), 1));
        QuickItem.AddRecipe(ModContent.ItemType<MysteriaWood>(), TileID.WorkBenches, 1, (Item.type, 4));
    }
}