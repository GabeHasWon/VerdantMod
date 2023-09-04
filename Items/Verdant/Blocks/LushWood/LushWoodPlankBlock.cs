using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.LushWood;

[Sacrifice(100)]
public class LushWoodPlankBlock : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 32, 18, ModContent.TileType<LushPlanks>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, TileID.Sawmill, 1, (ModContent.ItemType<VerdantWoodBlock>(), 1));
        QuickItem.AddRecipe(ModContent.ItemType<VerdantWoodBlock>(), TileID.Sawmill, 1, (Type, 1));
    }
}