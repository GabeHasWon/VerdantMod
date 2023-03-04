using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter.Fish;

[Sacrifice(3)]
class PoolwormItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Poolworm", "'Surprisingly slimy!'");

    public override void SetDefaults()
    {
        QuickItem.SetCritter(this, 22, 20, ModContent.NPCType<NPCs.Passive.Fish.Poolworm>(), 1, 13);
        Item.bait = 35;
    }

    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
