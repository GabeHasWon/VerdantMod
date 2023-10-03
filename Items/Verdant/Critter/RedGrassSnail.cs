using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(5)]
class RedGrassSnail : ModItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 34, 34, ModContent.NPCType<NPCs.Passive.VerdantRedGrassSnail>(), 1, 15, Item.buyPrice(0, 0, 15));
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
