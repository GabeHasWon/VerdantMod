using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.Puff;

[Sacrifice(3)]
public class PuffLampItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<Tiles.Verdant.Decor.MysteriaFurniture.MysteriaLamp>());
    public override void AddRecipes() 
        => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<LushLeaf>(), 3), (ModContent.ItemType<PuffMaterial>(), 3), (ModContent.ItemType<Lightbulb>(), 1));
}