using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Materials;
using Verdant.Items.Verdant.Tools;

namespace Verdant.Items.Verdant.Blocks.Bushes;

[Sacrifice(1)]
public class Bush3x3Item : ModItem
{
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ModContent.ItemType<Shears>());

    public override void SetDefaults() => QuickItem.SetBlock(this, 32, 36, ModContent.TileType<Tiles.Verdant.Decor.Bushes.TrimmingBush3x3>(), true, 0, ItemRarityID.Blue);
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<MysteriaWood>(), 6), (ModContent.ItemType<LushLeaf>(), 16));
}