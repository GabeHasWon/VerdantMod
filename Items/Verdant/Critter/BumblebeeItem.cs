using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(5)]
class BumblebeeItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 12, 12, ModContent.NPCType<NPCs.Passive.Bumblebee>(), 1, 5, Item.buyPrice(0, 0, 3, 50));
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
