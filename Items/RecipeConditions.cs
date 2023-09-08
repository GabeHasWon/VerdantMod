using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Tools;

namespace Verdant.Items;

internal class RecipeConditions : ILoadable
{
    public static Condition AfterApotheosisDownedPlantera;
    public static Condition HasShears;

    public void Load(Mod mod)
    {
        AfterApotheosisDownedPlantera = new Condition(Language.GetText("Mods.Verdant.RecipeCondition.AfterApotheosisDownedPlantera"), 
            () => ModContent.GetInstance<VerdantSystem>().apotheosisDowns["plantera"]);

        HasShears = new Condition(Language.GetText("Mods.Verdant.RecipeCondition.HasShears").WithFormatArgs(ModContent.ItemType<Shears>()),
            () => Main.LocalPlayer.HasItem(ModContent.ItemType<Shears>()));
    }

    public void Unload() => HasShears = AfterApotheosisDownedPlantera = null;
}
