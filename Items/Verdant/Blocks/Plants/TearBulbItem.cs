using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Food;
using Verdant.Items.Verdant.Materials;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks.Plants;

public class TearBulbItem : ApotheoticItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 28, 26, ModContent.TileType<TearBulb>(), rarity: ItemRarityID.Blue);
    public override void AddRecipes() => CreateRecipe().AddIngredient<LushLeaf>(10).AddIngredient<Waterberry>().AddCondition(Condition.InGraveyard).Register();

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(TearBulbItem))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.TearBulbItem.", 2, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.TearBulbItem.0").
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.TearBulbItem.1"));
    }
}
