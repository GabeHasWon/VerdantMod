using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantPianoItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Piano", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 40, 30, TileType<Tiles.Verdant.Decor.LushFurniture.LushPiano>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.LivingLoom, 1, (ItemType<LushLeaf>(), 10), (ItemType<RedPetal>(), 5), (ItemID.Book, 1), (ItemID.Bone, 4));
    }
}