using System.Collections.Generic;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace Verdant.Systems.Achievements;

// Any one boss item collected
public class GreenThumbAchievement : ModAchievement
{
    internal static CustomFlagCondition Condition = null;

    public override void SetStaticDefaults() => Condition = AddCondition();
}

// Half collected
public class GardenerAchievement : ModAchievement
{
    internal static CustomFlagCondition Condition = null;

    public override void SetStaticDefaults() => Condition = AddCondition();

    public override IEnumerable<Position> GetModdedConstraints()
    {
        yield return new After(ModContent.GetInstance<GreenThumbAchievement>());
    }
}

// All collected
public class ReachingApotheosisAchievement : ModAchievement
{
    internal static CustomFlagCondition Condition = null;

    public override void SetStaticDefaults() => Condition = AddCondition();

    public override IEnumerable<Position> GetModdedConstraints()
    {
        yield return new After(ModContent.GetInstance<GardenerAchievement>());
    }
}