using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Materials;

class WisplantItem : ApotheoticItem
{
    public override void SetDefaults() => QuickItem.SetMaterial(this, 16, 24, ItemRarityID.White, 999, false, Item.buyPrice(0, 0, 0, 5));

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(WisplantItem))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Wisplant.", 3, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Wisplant.0", 100).
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Wisplant.1", 80));
    }
}