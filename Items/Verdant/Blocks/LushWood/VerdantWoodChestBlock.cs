﻿using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    [Sacrifice(1)]
    public class VerdantWoodChestBlock : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.VerdantWoodChest>());

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<VerdantWoodBlock>(), 8)
                .AddRecipeGroup(RecipeGroupID.IronBar, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
