using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Aquamarine;

namespace Verdant.Items.Verdant.Blocks.Aquamarine;

public class GemsparkAquamarineItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Aquamarine Gemspark");
    public override void SetDefaults() => QuickItem.SetBlock(this, 24, 24, ModContent.TileType<GemsparkAquamarine>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 20, (ItemID.Glass, 20), (ModContent.ItemType<AquamarineItem>(), 1));
}
