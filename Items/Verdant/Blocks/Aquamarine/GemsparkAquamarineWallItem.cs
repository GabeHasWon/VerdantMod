using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Aquamarine;

public class GemsparkAquamarineWallItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Aquamarine Gemspark Wall");
    public override void SetDefaults() => Item.DefaultToPlacableWall((ushort)ModContent.WallType<GemsparkAquamarineWall>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, TileID.WorkBenches, 4, (ModContent.ItemType<GemsparkAquamarineItem>(), 1));
        QuickItem.AddRecipe(ModContent.ItemType<GemsparkAquamarineItem>(), TileID.WorkBenches, 1, (Type, 4));
    }
}
