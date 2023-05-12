using Terraria;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Materials;

class YellowBulb : ApotheoticItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Yellow Bulb", "'So rare...'");
    public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<YellowSprouts>());
    public override void Update(ref float gravity, ref float maxFallSpeed) => maxFallSpeed = 0.9f;

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(YellowBulb))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.YellowBulb.", 3, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.YellowBulb.0", 80).
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.YellowBulb.1", 100)).
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.YellowBulb.2", 80));
    }
}