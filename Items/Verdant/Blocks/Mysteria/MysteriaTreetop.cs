using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Trees;

namespace Verdant.Items.Verdant.Blocks.Mysteria;

public class MysteriaTreetop : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 54, 40, ModContent.TileType<MysteriaTreeTop>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<MysteriaWood>(), 4), (ModContent.ItemType<MysteriaClump>(), 8));
}
