using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    public class LushLampItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Lamp", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushLamp>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ModContent.ItemType<VerdantWoodBlock>(), 6), (ItemID.Torch, 1));
    }
}