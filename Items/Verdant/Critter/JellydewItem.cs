using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(3)]
class JellydewItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 18, 20, ModContent.NPCType<NPCs.Passive.Fish.Jellydew>(), 1, 5);
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
