using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture;

public class VerdantBathtub : BathtubTile<Items.Verdant.Blocks.VerdantFurniture.VerdantBathtubItem>
{
    protected override SpecificTileInfo SpecificInfo => new SpecificTileInfo(DustID.Grass, new Color(33, 142, 22));
}
