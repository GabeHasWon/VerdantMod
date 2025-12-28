using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks;

namespace Verdant.Systems.Achievements;

// Find the Statue of a Snail
public class SnailStatueAchievement : ModAchievement
{
    public override bool IsLoadingEnabled(Mod mod)
    {
        return false;
    }
    public override void SetStaticDefaults() => AddItemPickupCondition([ModContent.ItemType<SnailStatueItem>()]);
}