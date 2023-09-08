using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.Misc;

public class OvergrownBrickItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<OvergrownBricks>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.Anvils, 1, (ItemID.GrayBrick, 1), (ModContent.ItemType<LushLeaf>(), 1));
}

public class TrimmedOvergrownBrickItem : ModItem
{
    public override string Texture => base.Texture.Replace("Trimmed", "");

    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<TrimmedOvergrownBricks>());

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<OvergrownBrickItem>()
            .AddCondition(RecipeConditions.HasShears)
            .Register();
    }
}
