using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    [Sacrifice(1)]
    public class VerdantPianoItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 40, 30, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantPiano>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 10), (ModContent.ItemType<RedPetal>(), 5), (ItemID.Book, 1), (ItemID.Bone, 4));
    }
}