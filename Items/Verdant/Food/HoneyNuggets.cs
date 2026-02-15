using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.ScreenText;

namespace Verdant.Items.Verdant.Food;

public class HoneyNuggets : FoodItem
{
    internal override Color[] ParticleColors => [new Color(239, 137, 13)];
    internal override Point Size => new(28, 28);

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(HoneyNuggets))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
        {
            ApotheosisDialogueCache.Chat("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.HoneyNuggets", true);
            return null;
        }
        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.HoneyNuggets", true);
    }
}
