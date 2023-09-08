using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.Misc;

public class LightbulbLeafItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<LightbulbLeaves>());

    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<Lightbulb>().
            AddIngredient<LushLeaf>().
            AddTile(TileID.LivingLoom).
            Register();
    }
}
