using System;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantTableItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Table", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 40, 28, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantTable>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, Terraria.ID.TileID.WorkBenches, 1, (ItemType<LushLeaf>(), 6), (ItemType<VerdantStrongVineMaterial>(), 3), (ItemType<PinkPetal>(), 6));
    }
}