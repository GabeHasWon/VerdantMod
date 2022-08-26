using Microsoft.Xna.Framework;

namespace Verdant.Items.Verdant.Food;

public class BowlFruit : FoodItem
{
    internal override Point Size => new(40, 38);
    public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'Warter'");
}
