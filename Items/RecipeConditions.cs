using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Verdant.Items;

internal class RecipeConditions : ILoadable
{
    public static Condition AfterApotheosisDownedPlantera;

    public void Load(Mod mod)
    {
        AfterApotheosisDownedPlantera = new Condition(Language.GetText("Mods.Verdant.RecipeCondition.AfterApotheosisDownedPlantera"), 
            () => ModContent.GetInstance<VerdantSystem>().apotheosisDowns["plantera"]);
    }

    public void Unload() => AfterApotheosisDownedPlantera = null;
}
