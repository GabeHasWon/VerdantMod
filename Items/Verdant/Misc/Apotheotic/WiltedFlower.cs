using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Misc.Apotheotic;

[Sacrifice(1)]
class WiltedFlower : ApotheoticItem
{
    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Gray;
        Item.value = 0;
        Item.consumable = false;
        Item.width = 32;
        Item.height = 24;
    }

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(WiltedFlower))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.WiltedFlower.", 4, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.WiltedFlower.0").
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.WiltedFlower.1")).
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.WiltedFlower.2")).
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.WiltedFlower.3"));
    }
}
