using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.Mysteria;

public class MysteriaFluffItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<MysteriaFluff>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 2, (ModContent.ItemType<MysteriaClump>(), 1));
}
