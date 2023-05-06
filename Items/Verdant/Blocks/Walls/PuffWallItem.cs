using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Puff;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls;

public class PuffWallItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Puff Wall", "");
    public override void SetDefaults() => QuickItem.SetWall(this, 32, 32, ModContent.WallType<PuffWall>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, TileID.WorkBenches, 4, (ModContent.ItemType<PuffBlockItem>(), 1));
        QuickItem.AddRecipe(ModContent.ItemType<PuffBlockItem>(), TileID.WorkBenches, 1, (Type, 4));
    }
}