using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(3)]
class PuffSlimeSmallItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 16, 12, ModContent.NPCType<NPCs.Passive.Puff.PuffSlimeSmall>(), ItemRarityID.Green, 18, Item.buyPrice(0, 0, 4));
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
