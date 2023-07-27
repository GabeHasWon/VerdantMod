﻿using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    [Sacrifice(1)]
    public class VerdantChairItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantChair>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, Terraria.ID.TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 2), (ModContent.ItemType<PinkPetal>(), 2));
    }
}