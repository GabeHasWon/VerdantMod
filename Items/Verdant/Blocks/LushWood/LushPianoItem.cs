using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.LushWood
{
    [Sacrifice(1)]
    public class LushPianoItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 40, 30, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushPiano>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.Sawmill, 1, (ModContent.ItemType<VerdantWoodBlock>(), 15), (ItemID.Book, 1), (ItemID.Bone, 4));
    }
}