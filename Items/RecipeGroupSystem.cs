using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Aquamarine;
using Verdant.Items.Verdant.Critter;

namespace Verdant.Items;

internal class RecipeGroupSystem : ModSystem
{
    public static RecipeGroup AquamarineRecipeGroup;

    public override void Unload()
    {
        AquamarineRecipeGroup = null;
    }

    public override void AddRecipeGroups()
    {
        var set = new List<int>() { ModContent.ItemType<AquamarineItem>() };

        if (ModLoader.TryGetMod("ThoriumMod", out Mod thor))
            set.Add(thor.Find<ModItem>("Aquamarine").Type);

        AquamarineRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<AquamarineItem>())}", set.ToArray());
        RecipeGroup.RegisterGroup("Aquamarine", AquamarineRecipeGroup);

        RecipeGroup.recipeGroups[RecipeGroupID.Snails].ValidItems.Add(ModContent.ItemType<BulbSnail>());
        RecipeGroup.recipeGroups[RecipeGroupID.Snails].ValidItems.Add(ModContent.ItemType<RedGrassSnail>());
    }
}
