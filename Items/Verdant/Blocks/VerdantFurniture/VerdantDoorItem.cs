using System;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantDoorItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Door");
        public override void SetDefaults() => QuickItem.SetBlock(this, 42, 26, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantDoorClosed>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.LivingLoom, 1, (ItemType<RedPetal>(), 6), (ItemType<LushLeaf>(), 12));
    }
}