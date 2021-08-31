using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter
{
    class FolifishItem : ModItem
    {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Folifish");
        public override void SetDefaults() => QuickItem.SetCritter(this, 50, 34, ModContent.NPCType<NPCs.Passive.Folifish>(), 1, 13);
        public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
    }
}
