using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.ScreenText;

namespace Verdant.Items.Verdant.Food;

public class FloralTea : FoodItem
{
    internal override Color[] ParticleColors => [new Color(216, 187, 114), new(216, 54, 43)];
    internal override Point Size => new(26, 28);
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.CookingPots, 1, (ItemID.BottledWater, 1), (ModContent.ItemType<RedPetal>(), 2), (ModContent.ItemType<LushLeaf>(), 1));

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(FloralTea))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
        {
            ApotheosisDialogueCache.Chat("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.FloralTea", true);
            return null;
        }
        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.FloralTea", true);
    }
}
