using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Trees;

namespace Verdant.Items.Verdant.Blocks.Misc;

public class ChlorophytePlant : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Chloroflower Sprout", "Grows renewable chlorophyte\n'A jagged rock somehow brimming with life'");
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<ChlorophyteTree>());

    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient(ItemID.ChlorophyteOre, 5).
            AddIngredient(ModContent.ItemType<GreenCrystalItem>(), 1).
            AddCondition(Terraria.Localization.NetworkText.FromLiteral("Talk to the Apotheosis after beating Plantera"), 
                recipe => ModContent.GetInstance<VerdantSystem>().apotheosisDowns["plantera"]).
            Register();
    }
}
