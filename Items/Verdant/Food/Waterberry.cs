using Microsoft.Xna.Framework;

namespace Verdant.Items.Verdant.Food;

public class Waterberry : FoodItem
{
    internal override Point Size => new(28, 28);
    // public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'Almost like candy!'");
    internal override int BuffTime => 1 * 60 * 60; //1 minute
}
