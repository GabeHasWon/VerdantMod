using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class LushTableItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Table", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 40, 28, TileType<Tiles.Verdant.Decor.LushFurniture.LushTable>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, Terraria.ID.TileID.WorkBenches, 1, (ItemType<VerdantWoodBlock>(), 8));
    }
}
