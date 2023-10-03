using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.NPCs.Passive.Floties;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(3)]
class MysteriaFlotieItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 30, 38, ModContent.NPCType<MysteriaFlotie>(), ItemRarityID.Green, 20, Item.buyPrice(0, 0, 18));
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
