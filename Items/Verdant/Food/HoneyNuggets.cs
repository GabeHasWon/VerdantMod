using Microsoft.Xna.Framework;

namespace Verdant.Items.Verdant.Food;

public class HoneyNuggets : FoodItem
{
    internal override Point Size => new(28, 28);
    public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'Exceptionally good despite the origin'");
}
