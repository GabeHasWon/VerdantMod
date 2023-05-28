using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Verdant.Tiles.Verdant.Decor.MysteriaFurniture;

public class MysteriaBathtub : BathtubTile<Items.Verdant.Blocks.Mysteria.Furniture.MysteriaBathtubItem>
{
    protected override SpecificTileInfo SpecificInfo => new SpecificTileInfo(DustID.WoodFurniture, new Color(124, 93, 68));
}
