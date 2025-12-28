using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace Verdant.Systems.Achievements;

// Spawn a Marigold
public class MarigoldAchievement : ModAchievement
{
    internal static CustomFlagCondition Condition = null;

    public override void SetStaticDefaults() => Condition = AddCondition();
}