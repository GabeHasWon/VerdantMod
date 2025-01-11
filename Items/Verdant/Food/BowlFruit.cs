using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.ScreenText;

namespace Verdant.Items.Verdant.Food;

public class BowlFruit : FoodItem
{
    internal override Point Size => new(40, 38);

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(BowlFruit))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.BowlFruit.", 2, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.BowlFruit.0").
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.BowlFruit.1"));
    }
}
