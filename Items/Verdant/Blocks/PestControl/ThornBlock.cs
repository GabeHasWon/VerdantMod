using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.PestControl;

public class ThornBlock : ApotheoticItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<ThornTile>());

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<ThornWallItem>(), 4));
        QuickItem.AddRecipe(ModContent.ItemType<ThornWallItem>(), TileID.WorkBenches, 4, (Type, 1));
    }

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(ThornBlock))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ThornBlock.", 1, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ThornBlock.0");
    }
}
