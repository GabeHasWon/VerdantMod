using Terraria;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(5)]
class LushWingletItem : ApotheoticItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 26, 18, ModContent.NPCType<NPCs.Passive.SmallFly>(), 1, 5, Item.buyPrice(0, 0, 3));
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(LushWingletItem))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.LushWingletItem.", 1, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.LushWingletItem.0");
    }
}
