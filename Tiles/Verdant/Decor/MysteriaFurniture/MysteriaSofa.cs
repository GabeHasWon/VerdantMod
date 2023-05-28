using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Verdant.Tiles.Verdant.Decor.MysteriaFurniture;

internal class MysteriaSofa : SofaTile<Items.Verdant.Blocks.Mysteria.Furniture.MysteriaSofaItem>
{
    protected override SpecificTileInfo SpecificInfo => new(DustID.WoodFurniture, new(124, 93, 68));
}