using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;
using Verdant.Items.Verdant.Tools;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Materials;

class LushLeaf : ApotheoticItem
{
    public override void SetDefaults() => QuickItem.SetMaterial(this, 12, 12, ItemRarityID.White);

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<VerdantLeafWallItem>(), 4));

        QuickItem.AddRecipe(ItemID.BrightGreenDye, TileID.DyeVat, 1, (ModContent.ItemType<LushLeaf>(), 8), (ItemID.SilverDye, 1));
        QuickItem.AddRecipe(ItemID.GreenandBlackDye, TileID.DyeVat, 1, (ModContent.ItemType<LushLeaf>(), 8), (ItemID.BlackDye, 1));
    }

    /// <summary>Used to stop PermVineWand from using extra leaves.</summary>
    public override bool ConsumeItem(Player player) => player.HeldItem.type != ModContent.ItemType<PermVineWand>();

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(LushLeaf))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.LushLeaf.", 2, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.LushLeaf.0").
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.LushLeaf.1"));
    }
}
