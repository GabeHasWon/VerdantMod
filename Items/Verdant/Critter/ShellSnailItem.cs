using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(5)]
class ShellSnailItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 26, 22, ModContent.NPCType<NPCs.Passive.Snails.ShellSnail>(), 1, 5, Item.buyPrice(0, 0, 8));
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
