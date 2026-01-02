using System.Collections.Generic;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace Verdant.Systems.Achievements;

// Show the Apotheosis something
public class MudsquidAchievement : ModAchievement
{
    internal static CustomFlagCondition Condition = null;

    public override void SetStaticDefaults() => Condition = AddCondition();

    public override IEnumerable<Position> GetModdedConstraints()
    {
        yield return new After(ModContent.GetInstance<ReachingApotheosisAchievement>());
    }
}