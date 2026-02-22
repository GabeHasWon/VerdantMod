using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Dusts;
using Verdant.Items;
using Verdant.Items.Verdant;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Decor.PuffFurniture;

public class PuffBathtub : BathtubTile<PuffBathtub.PuffBathtubItem>
{
    [Sacrifice(1)]
    public class PuffBathtubItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, ModContent.TileType<PuffBathtub>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<LushLeaf>(), 6), (ModContent.ItemType<PuffMaterial>(), 8));
    }

    protected override SpecificTileInfo SpecificInfo => new SpecificTileInfo(ModContent.DustType<PuffDust>(), new Color(255, 112, 202));
}
