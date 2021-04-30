using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter
{
    class FlotieItem : ModItem
    {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Flotie");
        public override void SetDefaults() => QuickItem.SetCritter(this, 34, 34, ModContent.NPCType<NPCs.Verdant.Passive.Flotie>(), 1, 13);
    }
}
