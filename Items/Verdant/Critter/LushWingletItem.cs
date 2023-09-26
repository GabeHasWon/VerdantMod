using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(5)]
class LushWingletItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 26, 18, ModContent.NPCType<NPCs.Passive.SmallFly>(), 1, 5);
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
