using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.ScreenText;

namespace Verdant.Items.Verdant.Food;

public class Waterberry : FoodItem
{
    internal override Point Size => new(28, 28);
    // public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'Almost like candy!'");
    internal override int BuffTime => 1 * 60 * 60; //1 minute

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(Waterberry))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
        {
            ApotheosisDialogueCache.Chat("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Waterberry", true);
            return null;
        }
        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Waterberry", true);
    }
}
