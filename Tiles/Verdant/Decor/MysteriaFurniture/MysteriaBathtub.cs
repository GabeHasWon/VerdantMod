using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Dusts;

namespace Verdant.Tiles.Verdant.Decor.MysteriaFurniture;

public class MysteriaBathtub : BathtubTile<Items.Verdant.Blocks.Mysteria.Furniture.MysteriaBathtubItem>
{
    protected override SpecificTileInfo SpecificInfo => new SpecificTileInfo(ModContent.DustType<MysteriaWoodDust>(), new Color(124, 93, 68));
}
