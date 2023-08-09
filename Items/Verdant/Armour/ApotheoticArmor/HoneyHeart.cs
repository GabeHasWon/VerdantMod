using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Armour.ApotheoticArmor;

[Sacrifice(0)]
public class HoneyHeart : ModItem
{
    public override void SetStaticDefaults() => ItemID.Sets.IgnoresEncumberingStone[Type] = true;

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 18;
        Item.maxStack = 1;
    }

    public override bool ItemSpace(Player player) => true;

    public override bool OnPickup(Player player)
    {
        player.Heal(10);
        return false;
    }
}