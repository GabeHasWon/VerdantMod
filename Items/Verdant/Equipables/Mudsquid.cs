using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Players;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.ScreenText;

namespace Verdant.Items.Verdant.Equipables;

class Mudsquid : ApotheoticItem
{
    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(silver: 3);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<MudsquidPlayer>().hasSquid = true;

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(Mudsquid))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Mudsquid.", 3, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Mudsquid.0").
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Mudsquid.1")).
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Mudsquid.2"));
    }
}