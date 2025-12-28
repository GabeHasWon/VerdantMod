using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace Verdant.Systems.Achievements;

// Show the Apotheosis something
public class ShowcaseAchievement : ModAchievement
{
    internal static CustomFlagCondition Condition = null;

    public override void SetStaticDefaults() => Condition = AddCondition();
}