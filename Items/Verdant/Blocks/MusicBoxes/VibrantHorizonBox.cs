using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Decor.MusicBox;

namespace Verdant.Items.Verdant.Blocks.MusicBoxes;

[Sacrifice(1)]
public class VibrantHorizonBox : BaseMusicBoxItem<VibrantHorizonBoxTile>
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/VibrantHorizon"), ModContent.ItemType<VibrantHorizonBox>(), ModContent.TileType<VibrantHorizonBoxTile>());
    }
}