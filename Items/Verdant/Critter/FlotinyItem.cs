using Terraria;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(5)]
class FlotinyItem : ApotheoticItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 22, 26, ModContent.NPCType<NPCs.Passive.Floties.Flotiny>(), 1, 8);
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(FlotinyItem))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.FlotinyItem.", 1, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.FlotinyItem.0");
    }
}
