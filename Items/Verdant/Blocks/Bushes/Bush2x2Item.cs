using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.Bushes;

[Sacrifice(1)]
public class Bush2x2Item : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 32, 36, ModContent.TileType<Tiles.Verdant.Decor.Bushes.TrimmingBush2x2>(), true, 0, ItemRarityID.Blue);
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<MysteriaWood>(), 4), (ModContent.ItemType<LushLeaf>(), 10));
}