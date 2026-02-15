using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.ScreenText;

namespace Verdant.Items.Verdant.Food;

public class Mystuber : FoodItem
{
    internal override Color[] ParticleColors => [new Color(130, 111, 80), new Color(39, 113, 101)];
    internal override Point Size => new(30, 40);

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(Mystuber))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
        {
            ApotheosisDialogueCache.Chat("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Mystuber", true);
            return null;
        }
        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Mystuber", true);
    }
}
