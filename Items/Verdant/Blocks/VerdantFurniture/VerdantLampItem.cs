using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantLampItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Lamp", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantLamp>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 4), (ModContent.ItemType<VerdantStrongVineMaterial>(), 3), (ItemID.Torch, 1));
    }
}