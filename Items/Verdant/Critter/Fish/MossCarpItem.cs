using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter.Fish;

[Sacrifice(3)]
class MossCarpItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Moss Carp", "'A mix of hearty meat and grass'");
    public override void SetDefaults() => QuickItem.SetCritter(this, 22, 20, ModContent.NPCType<NPCs.Passive.Fish.MossCarp>(), 1, 13);
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
