using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Dusts;

namespace Verdant.Tiles.Verdant.Decor.MysteriaFurniture;

internal class MysteriaSofa : SofaTile<Items.Verdant.Blocks.Mysteria.Furniture.MysteriaSofaItem>
{
    protected override SpecificTileInfo SpecificInfo => new(ModContent.DustType<MysteriaWoodDust>(), new(124, 93, 68));
}