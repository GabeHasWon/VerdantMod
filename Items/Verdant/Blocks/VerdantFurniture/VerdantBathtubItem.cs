﻿using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    [Sacrifice(1)]
    public class VerdantBathtubItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantBathtub>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 10), (ModContent.ItemType<RedPetal>(), 4));
    }
}