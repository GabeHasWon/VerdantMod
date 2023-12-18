using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.ScreenText;

namespace Verdant.Items.Verdant.Equipables;

class ExpertPlantGuide : ApotheoticItem
{
    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(gold: 5);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<VerdantPlayer>().expertPlantGuide = true;

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(ExpertPlantGuide))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ExpertPlantGuide.", 3, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ExpertPlantGuide.0").
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ExpertPlantGuide.1")).
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ExpertPlantGuide.2"));
    }
}
