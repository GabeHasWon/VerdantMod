using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class LushWorkbenchItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Workbench", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 30, 16, TileType<Tiles.Verdant.Decor.LushFurniture.LushWorkbench>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, -1, 1, (ItemType<VerdantWoodBlock>(), 10));
    }
}