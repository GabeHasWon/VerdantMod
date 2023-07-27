using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(3)]
class MysteriaFlotinyItem : ModItem
{
    // public override void SetStaticDefaults() => DisplayName.SetDefault("Mysteria Flotiny");
    public override void SetDefaults() => QuickItem.SetCritter(this, 22, 26, ModContent.NPCType<NPCs.Passive.Floties.MysteriaFlotiny>(), ItemRarityID.Green, 18);
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
