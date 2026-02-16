using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks.Plants;

public class SmokeBulbItem : ApotheoticItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 28, 26, ModContent.TileType<SmokeBulb>(), rarity: ItemRarityID.Blue);
    public override void AddRecipes() => CreateRecipe().AddIngredient<LushLeaf>(15).AddCondition(Condition.InGraveyard).Register();

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(SmokeBulbItem))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.SmokeBulbItem.", 2, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.SmokeBulbItem.0").
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.SmokeBulbItem.1"));
    }
}
