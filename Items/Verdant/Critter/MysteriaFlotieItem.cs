using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(3)]
class MysteriaFlotieItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 30, 38, ModContent.NPCType<NPCs.Passive.Floties.MysteriaFlotie>(), ItemRarityID.Green, 20);
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
