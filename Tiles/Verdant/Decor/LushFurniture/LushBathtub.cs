using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Verdant.Tiles.Verdant.Decor.LushFurniture;

public class LushBathtub : BathtubTile<Items.Verdant.Blocks.LushWood.LushBathtubItem>
{
    protected override SpecificTileInfo SpecificInfo => new SpecificTileInfo(DustID.WoodFurniture, new Color(114, 69, 39));
}