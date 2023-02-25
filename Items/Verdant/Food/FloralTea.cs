using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Food;

public class FloralTea : FoodItem
{
    internal override Point Size => new(26, 28);
    public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'Wonderfully sweet'");
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.CookingPots, 1, (ItemID.BottledWater, 1), (ModContent.ItemType<RedPetal>(), 2), (ModContent.ItemType<LushLeaf>(), 1));
}
