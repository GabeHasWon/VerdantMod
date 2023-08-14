using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Armour.ApotheoticArmor;

[Sacrifice(0)]
public class FruitIconYellow : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 14;
        Item.height = 18;
        Item.maxStack = 1;
    }
}

public class FruitIconGreen : FruitIconYellow { }
public class FruitIconRed : FruitIconYellow { }