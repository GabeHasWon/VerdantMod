using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.ScreenText;

namespace Verdant.Items.Verdant.Food;

public class Grapecherry : FoodItem
{
    internal override Point Size => new(24, 28);

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(Grapecherry))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Grapecherry.", 2, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Grapecherry.0").
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Grapecherry.1"));
    }
}
