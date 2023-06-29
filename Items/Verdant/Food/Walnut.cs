using Microsoft.Xna.Framework;

namespace Verdant.Items.Verdant.Food;

public class Walnut : FoodItem
{
    internal override Point Size => new(30, 26);
    public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'Your teeth will see better days'");
}
