using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(3)]
class PuffSlimeSmallItem : ModItem
{
    public override void SetStaticDefaults() => DisplayName.SetDefault("Puff Slime");
    public override void SetDefaults() => QuickItem.SetCritter(this, 16, 12, ModContent.NPCType<NPCs.Passive.Puff.PuffSlimeSmall>(), ItemRarityID.Green, 18);
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
