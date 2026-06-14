using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Dusts;
using Verdant.Items;
using Verdant.Items.Verdant;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Decor.PuffFurniture;

public class PuffSofa : SofaTile<Items.Verdant.Blocks.Mysteria.Furniture.MysteriaSofaItem>
{
    [Sacrifice(3)]
    public class PuffSofaItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 48, 28, ModContent.TileType<PuffSofa>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.Sawmill, 1, (ModContent.ItemType<PuffMaterial>(), 3), (ModContent.ItemType<LushLeaf>(), 3), (ItemID.Silk, 1));
    }

    protected override SpecificTileInfo SpecificInfo => new(ModContent.DustType<PuffDust>(), new Color(255, 112, 202));
}