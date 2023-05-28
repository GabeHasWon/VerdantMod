using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture;

internal class VerdantSofa : SofaTile<Items.Verdant.Blocks.VerdantFurniture.VerdantSofaItem>
{
    protected override SpecificTileInfo SpecificInfo => new SpecificTileInfo(DustID.Grass, new Color(33, 142, 22));
}