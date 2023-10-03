using Terraria;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(3)]
class FlotieItem : ApotheoticItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 34, 34, ModContent.NPCType<NPCs.Passive.Floties.Flotie>(), 1, 13, Item.buyPrice(0, 0, 9));
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(FlotieItem))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.FlotieItem.", 1, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.FlotieItem.0");
    }
}
