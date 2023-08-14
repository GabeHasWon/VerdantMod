using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Misc.Apotheotic;

public class ApotheosisBook : ApotheoticItem
{
    public override void SetDefaults() => QuickItem.SetMaterial(this, 36, 46, ItemRarityID.Purple, 1, false, 0, true);
    public override bool AltFunctionUse(Player player) => true;

    public override bool? UseItem(Player player)
    {
        if (player.altFunctionUse == 2 && player.itemAnimation > 14)
        {
            QuickItem.ToggleBookUI(Language.GetTextValue("Mods.Verdant.Books.ApotheosisBook.Title"), 0.8f,
                new object[] { Language.GetTextValue("Mods.Verdant.Books.ApotheosisBook.Content"),
                ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>("Verdant/Systems/UI/Textures/Signature", ReLogic.Content.AssetRequestMode.ImmediateLoad) });
            return true;
        }

        Item.placeStyle = Main.rand.Next(2) + 4;
        return null;
    }

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(ApotheosisBook))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheosisBook.", 6, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheosisBook.0").
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheosisBook.1")).
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheosisBook.2")).
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheosisBook.3")).
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheosisBook.4")).
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheosisBook.5"));
    }
}
