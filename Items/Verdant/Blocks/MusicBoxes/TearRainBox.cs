using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Decor.MusicBox;

namespace Verdant.Items.Verdant.Blocks.MusicBoxes;

[Sacrifice(1)]
public class TearRainBox : BaseMusicBoxItem<TearRainBoxTile>
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/TearRain"), ModContent.ItemType<TearRainBox>(), ModContent.TileType<TearRainBoxTile>());
    }
}