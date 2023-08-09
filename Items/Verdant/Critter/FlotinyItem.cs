using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter
{
    [Sacrifice(5)]
    class FlotinyItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetCritter(this, 22, 26, ModContent.NPCType<NPCs.Passive.Floties.Flotiny>(), 1, 8);
        public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
    }
}
