using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter
{
    class FlotinyItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flotiny");
            Tooltip.SetDefault("'Oh my god it's so tiny'");
        }

        public override void SetDefaults() => QuickItem.SetCritter(this, 22, 26, ModContent.NPCType<NPCs.Verdant.Passive.Flotiny>(), 1, 8);
        public override bool CanUseItem(Player player) => !Framing.GetTileSafely(Main.MouseWorld).active();
    }
}
