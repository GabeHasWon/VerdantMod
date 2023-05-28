using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Verdant.Tiles.Verdant.Decor.LushFurniture;

internal class LushSofa : SofaTile<Items.Verdant.Blocks.LushWood.LushWoodSofaItem>
{
    protected override SpecificTileInfo SpecificInfo => new SpecificTileInfo(DustID.WoodFurniture, new(114, 69, 39));
}