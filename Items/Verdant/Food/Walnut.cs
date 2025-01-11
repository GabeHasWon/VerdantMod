using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.ScreenText;

namespace Verdant.Items.Verdant.Food;

public class Walnut : FoodItem
{
    internal override Point Size => new(30, 26);

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(Walnut))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
        {
            ApotheosisDialogueCache.Chat("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Walnut", true);
            return null;
        }
        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Walnut", true);
    }
}
