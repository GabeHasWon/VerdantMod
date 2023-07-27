﻿using Terraria.ModLoader;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls
{
    public class LushSoilWallItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetWall(this, 32, 32, ModContent.WallType<LushSoilWall>());
        public override void AddRecipes()
        {
            QuickItem.AddRecipe(this, -1, 4, (ModContent.ItemType<LushSoilBlock>(), 1));
            QuickItem.AddRecipe(ModContent.ItemType<LushSoilBlock>(), -1, 1, (Item.type, 4));
        }
    }
}