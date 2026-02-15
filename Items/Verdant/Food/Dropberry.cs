using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.ScreenText;

namespace Verdant.Items.Verdant.Food;

public class Dropberry : FoodItem
{
    internal override Color[] ParticleColors => [new Color(85, 144, 209), new(55, 182, 78)];
    internal override Point Size => new(38, 40);

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(Dropberry))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Dropberry.", 2, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Dropberry.0").
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Dropberry.1"));
    }
}
