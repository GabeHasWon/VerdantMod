using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Trees;

namespace Verdant.Items.Verdant.Blocks.Mysteria;

public class MysteriaTreetop : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Mysteria Canopy", "Places a Mysteria Tree treetop\n'Some assembly required'");
    public override void SetDefaults() => QuickItem.SetBlock(this, 54, 40, ModContent.TileType<MysteriaTreeTop>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<MysteriaWood>(), 4));
}
