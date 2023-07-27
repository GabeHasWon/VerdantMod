﻿using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    [Sacrifice(1)]
    public class VerdantBookshelfItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 38, 52, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantBookshelf>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 14), (ModContent.ItemType<RedPetal>(), 4), (ModContent.ItemType<PinkPetal>(), 4), (ItemID.Book, 8));
    }
}