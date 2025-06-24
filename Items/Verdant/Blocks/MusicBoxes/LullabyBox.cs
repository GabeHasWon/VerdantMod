using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Decor.MusicBox;

namespace Verdant.Items.Verdant.Blocks.MusicBoxes;

[Sacrifice(1)]
public class LullabyBox : BaseMusicBoxItem<LullabyBoxTile>
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ApotheosisLullaby"), ModContent.ItemType<LullabyBox>(), ModContent.TileType<LullabyBoxTile>());
    }
}