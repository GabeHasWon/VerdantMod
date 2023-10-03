using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.NPCs.Passive.Floties;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(3)]
class MysteriaFlotinyItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 22, 26, ModContent.NPCType<MysteriaFlotiny>(), ItemRarityID.Green, 18, Item.buyPrice(0, 0, 12));
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
