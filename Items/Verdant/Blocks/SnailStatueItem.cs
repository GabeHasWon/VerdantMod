using Terraria;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.TearRain;
using Verdant.Tiles.Verdant.Decor;

namespace Verdant.Items.Verdant.Blocks;

[Sacrifice(1)]
public class SnailStatueItem : ApotheoticItem
{
	public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<SnailStatue>());

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(SnailStatueItem))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        bool canDoRain = TearRainSystem.DaysSinceArtificialRain >= 3;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
        {
            if (canDoRain)
            {
                TearRainSystem.StartRain(1, -1, true);
                return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.SnailStatueItem.", 2, true);
            }
            else
            {
                ApotheosisDialogueCache.Chat("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.SnailStatueItem.TooSoon");
                return null;
            }
        }

        if (canDoRain) 
        {
            return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.SnailStatueItem.0").
                FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.SnailStatueItem.1"), (_) => TearRainSystem.StartRain(1, -1, true));
        }

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.SnailStatueItem.TooSoon", true);
    }
}