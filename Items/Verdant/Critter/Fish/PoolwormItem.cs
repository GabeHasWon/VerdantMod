using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter.Fish;

[Sacrifice(3)]
class PoolwormItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 22, 20, ModContent.NPCType<NPCs.Passive.Fish.Poolworm>(), 1, 35, Item.buyPrice(0, 0, 4));
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
