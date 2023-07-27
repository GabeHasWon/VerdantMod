using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter.Fish;

[Sacrifice(3)]
class AxolotlItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 22, 20, ModContent.NPCType<NPCs.Passive.Fish.Axolotl>(), ItemRarityID.Blue, 0);
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
